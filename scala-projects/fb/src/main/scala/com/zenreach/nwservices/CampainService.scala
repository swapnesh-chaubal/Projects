package com.zenreach.nwservices

import com.twitter.app.Flag
import com.twitter.conversions.time
import com.twitter.finagle.{Http, Service}
import com.twitter.finagle.http.{Request, Response}
import com.twitter.finagle.param.Stats
import com.twitter.server.TwitterServer
import com.twitter.util.{Await, Future}
import io.finch._
import io.finch.circe._
import io.finch.syntax._
import io.circe.generic.auto._
import com.twitter.util.Future
import com.zenreach.nwservices.campaigns.entities.{AdCreative, TargetingSpec}

case class Message(message: String)

case class FullName(firstName: String, lastName: String)

/*
{
	"location_id": "59d7c747a6335e0009ce6f04",
	"facebook": {
		"user_account_id": "108401719980306",
		"ads_account_id": "",
		"app_secret": "04e15624ccaa918c5b0c94ff7ed6b638",
		"access_token": "EAACWGJMRw7kBACWPZAoNAFLszDxGvUWaKnLJu74XiLhRlBZBP5ELtEwCC5QzHf2ZBW0u5XlMMkk7ikL4s6awZCHZAftZA5dlo1RKON596BOVoZBw24ZADHnLZCwy1oU70QxCqyIfCO47ILlxmW9FGLhczfqpCiunc2ggZD",
		"campaign": {
			"create_campaign_request": {
				"name": "string",
				"objective": "NONE",
				"status": "PAUSED"
			},
			"ad_sets": [{
				"create_ad_set_request": {
					"campaign_id": "string",
					"daily_budget": "string",
					"end_time": "2018-04-06T21:29:03.170Z",
					"is_autobid": true,
					"name": "string",
					"optimization_goal": "POST_ENGAGEMENT",
					"start_time": "2018-04-06T21:29:03.170Z",
					"status": "PAUSED",
					"targeting": {
						"geo_locations": {
							"custom_locations": [{
								"name": "string",
								"address_string": "string",
								"distance_unit": "string",
								"latitude": "string",
								"longitude": "string",
								"radius": "string",
								"country": "string"
							}],
							"location_types": [
								"string"
							]
						},
						"age_min": 0,
						"age_max": 0,
						"custom_audiences": [
							"string"
						],
						"publisher_platforms": [
							"string"
						],
						"facebook_positions": [
							"string"
						],
						"instagram_positions": [
							"string"
						],
						"device_platforms": [
							"string"
						]
					}
				},
				"ads": [{
					"ad_creative": {
						"create_ad_creative_request": {
							"body": "string",
							"image_url": "string",
							"link_url": "string",
							"name": "string",
							"object_story_spec": {
								"page_id": "string",
								"link_data": {
									"link": "string",
									"message": "string",
									"caption": "string",
									"name": "string",
									"description": "string",
									"picture_url": "string"
								}
							},
							"title": "string"
						}
					},
					"create_ad_request": {
						"name": "string",
						"status": "PAUSED"
					}
				}],
				"targeting_type": "ZR_SEED"
			}]
		}
	}
}
 */
class CampaignService {
  def createCampaign(
      incomingMessage: Seq[TargetingSpec]): Future[Seq[TargetingSpec]] =
    Future.value(incomingMessage)

  def createAdCreative(incomingMessage: AdCreative): Future[String] =
    Future.value(
      FacebookCampaignCreator.createAdCreative(
        incomingMessage.fb_info,
        incomingMessage.object_story_spec.link_data.message,
        incomingMessage.object_story_spec.link_data.picture_url,
        incomingMessage.object_story_spec.link_data.link,
        incomingMessage.object_story_spec.page_id
      ).getId
    )
}

object CampaignServer extends TwitterServer {
  val port: Flag[Int] = flag("port", 8081, "TCP port for HTTP server")

  val campaignService = new CampaignService

  def createCampaignEndpoint: Endpoint[Seq[TargetingSpec]] =
    jsonBody[Seq[TargetingSpec]]

  def createCampaign: Endpoint[Seq[TargetingSpec]] =
    post("create_campaign" :: createCampaignEndpoint) {
      incomingMessage: Seq[TargetingSpec] =>
        campaignService.createCampaign(incomingMessage).map(Ok)
    }

  def createAdCreativeEndpoint: Endpoint[String] = jsonBody[AdCreative]

  def createAdCreative: Endpoint[AdCreative] =
    post("create_ad_creative" :: createAdCreativeEndpoint) {
      incomingMessage: AdCreative =>
        campaignService.createAdCreative(incomingMessage).map(Ok)
    }

  val api = (createCampaign :+: createAdCreative).handle {
    case e: Exception => InternalServerError(e)
  }

  def main(): Unit = {

    val server = Http.server
      .withStatsReceiver(statsReceiver)
      .serve(s":${port()}", api.toServiceAs[Application.Json])

    closeOnExit(server)

    Await.ready(adminHttpServer)
  }
}
