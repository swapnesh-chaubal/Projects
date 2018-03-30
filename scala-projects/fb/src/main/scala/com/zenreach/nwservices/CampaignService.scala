package com.zenreach.nwservices

import com.facebook.ads.sdk.Targeting
import com.facebook.ads.sdk._
import com.zenreach.nwservices.campaigns.entities.{
  FacebookConversions,
  TargetingSpec
}

import scala.concurrent.{ExecutionContext, Future}
import org.json4s.jackson.JsonMethods._
import org.json4s.DefaultFormats
import com.twitter.bijection.Conversion._

import scala.collection.immutable

object FacebookParams {
  val accessToken =
    "EAACWGJMRw7kBACWPZAoNAFLszDxGvUWaKnLJu74XiLhRlBZBP5ELtEwCC5QzHf2ZBW0u5XlMMkk7ikL4s6awZCHZAftZA5dlo1RKON596BOVoZBw24ZADHnLZCwy1oU70QxCqyIfCO47ILlxmW9FGLhczfqpCiunc2ggZD"
  val appId = "165032290796473"
  val appSecret = "04e15624ccaa918c5b0c94ff7ed6b638"
  val accountId = "108401719980306"

  val ts =
    """
      |[
      |	{
      | "targeting_type": "ZR_SEED",
      |		"age_max": 60,
      |		"age_min": 21,
      |		"geo_locations": {
      |			"countries": ["US"],
      |			"location_types": ["home", "recent"],
      |			"custom_locations": [{
      |				"radius": "5",
      |				"country": "US",
      |				"latitude": "-7.36153152E+01",
      |				"longitude": "4.06843847E+01",
      |				"distance_unit": "mile"
      |			}]
      |		},
      |		"custom_audiences": [
      |			"a1"
      |		],
      |		"device_platforms": ["VALUE_MOBILE", "VALUE_DESKTOP"],
      |		"facebook_positions": ["feed", "instant_article"],
      |		"instagram_positions": ["stream"],
      |		"publisher_platforms": ["facebook", "instagram"]
      |	},
      |	{
      | "targeting_type": "FB_CONTROL",
      |		"age_max": 60,
      |		"age_min": 21,
      |		"geo_locations": {
      |			"countries": ["US"],
      |			"location_types": ["home", "recent"],
      |			"custom_locations": [{
      |				"radius": "5",
      |				"country": "US",
      |				"latitude": "-7.36153152E+01",
      |				"longitude": "4.06843847E+01",
      |				"distance_unit": "mile"
      |			}]
      |		},
      |		"custom_audiences": [
      |			"a2"
      |		],
      |		"device_platforms": ["VALUE_MOBILE", "VALUE_DESKTOP"],
      |		"facebook_positions": ["feed", "instant_article"],
      |		"instagram_positions": ["stream"],
      |		"publisher_platforms": ["facebook", "instagram"]
      |	},
      |	{
      | "targeting_type": "FB_IG_CONTROL",
      |		"age_max": 60,
      |		"age_min": 21,
      |		"geo_locations": {
      |			"countries": ["US"],
      |			"location_types": ["home", "recent"],
      |			"custom_locations": [{
      |				"radius": "5",
      |				"country": "US",
      |				"latitude": "-7.36153152E+01",
      |				"longitude": "4.06843847E+01",
      |				"distance_unit": "mile"
      |			}]
      |		},
      |		"custom_audiences": ["a1", "a2"],
      |		"device_platforms": ["VALUE_MOBILE", "VALUE_DESKTOP"],
      |		"facebook_positions": ["feed", "instant_article"],
      |		"instagram_positions": ["stream"],
      |		"publisher_platforms": ["facebook", "instagram"]
      |	}
      |]
    """.stripMargin
}

class CampaignService(implicit val executionContext: ExecutionContext)
    extends FacebookConversions {

  def getTargetingSpec(specJson: String): immutable.Seq[TargetingSpec] = {
    implicit val formats = DefaultFormats
    parse(specJson).extract[List[TargetingSpec]]

  }

  def createSingleAdSetFacebook(account: AdAccount,
                                campaignId: String,
                                targetingSpec: TargetingSpec): AdSet = {
    account
      .createAdSet()
      .setName(s"Adset ${targetingSpec.targeting_type} for $campaignId")
      .setCampaignId(campaignId)
      .setStatus(AdSet.EnumStatus.VALUE_PAUSED)
      .setBillingEvent(AdSet.EnumBillingEvent.VALUE_IMPRESSIONS)
      .setOptimizationGoal(AdSet.EnumOptimizationGoal.VALUE_IMPRESSIONS)
      .setTargeting(targetingSpec.as[Targeting])
      .execute()
  }

  def createAdSetsFacebook(account: AdAccount,
                           campaignId: String,
                           targetingSpec: Seq[TargetingSpec]): Seq[AdSet] =
    for (ts <- targetingSpec)
      yield createSingleAdSetFacebook(account, campaignId, ts)

  def createCampaign(): Future[Campaign] = {
    val context = new APIContext(FacebookParams.accessToken,
                                 FacebookParams.appSecret).enableDebug(true)
    val account: AdAccount = new AdAccount(FacebookParams.accountId, context)
    val tSpec = getTargetingSpec(FacebookParams.ts)

    Future {
      account
        .createCampaign()
        .setName("Swapnesh's scala campaign2")
        .setStatus(Campaign.EnumStatus.VALUE_PAUSED)
        .setObjective(Campaign.EnumObjective.VALUE_POST_ENGAGEMENT)
        .execute()

    }
  }

  def getCampaign(): Future[Option[Campaign]] = ???

}

object TestTargeting extends FacebookConversions {

  def getTargetingSpec(specJson: String): immutable.Seq[TargetingSpec] = {
    implicit val formats = DefaultFormats
    parse(specJson).extract[List[TargetingSpec]]

  }

  def createCampaign(targetingSpecs: Seq[TargetingSpec]): Campaign = {
    val context = new APIContext(FacebookParams.accessToken,
                                 FacebookParams.appSecret).enableDebug(true)
    val account: AdAccount = new AdAccount(FacebookParams.accountId, context)

    val campaign = account
      .createCampaign()
      .setName("Swapnesh's scala campaign2")
      .setStatus(Campaign.EnumStatus.VALUE_PAUSED)
      .setObjective(Campaign.EnumObjective.VALUE_POST_ENGAGEMENT)
      .execute()

    targetingSpecs.foreach(ts => {
      val targeting = ts.as[Targeting]
      account
        .createAdSet()
        .setName("Java SDK Test AdSet")
        .setCampaignId(campaign.getFieldId())
        .setStatus(AdSet.EnumStatus.VALUE_PAUSED)
        .setBillingEvent(AdSet.EnumBillingEvent.VALUE_IMPRESSIONS)
        .setOptimizationGoal(AdSet.EnumOptimizationGoal.VALUE_IMPRESSIONS)
        .setTargeting(targeting)
        .execute()
    })
    campaign
  }

}
