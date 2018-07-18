package com.zenreach.nwservices.campaigns.entities

case class FacebookAccount(
    access_token: String,
    app_secret: String,
    account_id: String
)

case class AdCreativeLinkData(
    link: String,
    message: String,
    caption: Option[String],
    name: Option[String],
    description: Option[String],
    picture_url: String
)

case class ObjectStorySpec(
    page_id: String,
    link_data: AdCreativeLinkData
)

case class AdCreative(name: String,
                      fb_info: FacebookAccount,
                      object_story_spec: ObjectStorySpec,
                      title: Option[String])

case class AdSet(daily_budget: String,
                 start_time: String,
                 end_time: String,
                 status: String,
                 is_autobid: Boolean,
                 targeting: TargetingSpec)


/*
{
  "fb_info": {
    "access_token": "EAAaZA0NzZAa9oBAEBuGaHYLGBhI8sWylZCy5V9BW8FoZAu2g73j5Pn2a5zuid9Ujbv3qo3fmphn8f5jsYGYqQ8CR8CtdFf7I8uEXpqzXWNX2MsKBAcivvH4TAHWhEmKkkzlWA1uiCXfZBcZB2bZCyFvMb0BEokUWx4ZD",
    "app_secret": "8cdfbf7478ca2484368a819a314b4d1b",
    "account_id": "126959897820002"
  }
  "name": "Swapnesh's test ad creative",
  "object_story_spec": {
    "page_id": "1772494609456362",
    "link_data": {
      "picture_url": "https://zp-uw2-nwreporting.s3.us-west-2.amazonaws.com/COW_20180401T223949779Z.jpg",
      "link": "www.zenreach.com",
      "message": "This offer Rocks"
    }
  }
}
 */
case class Campaign(name: String, objective: String, status: String)

case class FacebookChannel(user_account_id: String,
                           app_secret: String,
                           access_token: String)

case class CampaignRequest(location_id: String)
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
