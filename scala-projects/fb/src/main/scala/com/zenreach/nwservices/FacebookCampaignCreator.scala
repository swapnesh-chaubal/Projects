package com.zenreach.nwservices

import com.facebook.ads.sdk.Targeting
import com.facebook.ads.sdk._
import com.zenreach.nwservices.campaigns.entities.{
  FacebookAccount,
  FacebookConversions,
  TargetingSpec
}

import scala.concurrent.Future
import org.json4s.jackson.JsonMethods._
import org.json4s.DefaultFormats
import com.twitter.bijection.Conversion._

import scala.collection.immutable

object FacebookParams {
//  val accessToken =
//    "EAACWGJMRw7kBACWPZAoNAFLszDxGvUWaKnLJu74XiLhRlBZBP5ELtEwCC5QzHf2ZBW0u5XlMMkk7ikL4s6awZCHZAftZA5dlo1RKON596BOVoZBw24ZADHnLZCwy1oU70QxCqyIfCO47ILlxmW9FGLhczfqpCiunc2ggZD"
  val accessToken =
    "EAAaZA0NzZAa9oBAEBuGaHYLGBhI8sWylZCy5V9BW8FoZAu2g73j5Pn2a5zuid9Ujbv3qo3fmphn8f5jsYGYqQ8CR8CtdFf7I8uEXpqzXWNX2MsKBAcivvH4TAHWhEmKkkzlWA1uiCXfZBcZB2bZCyFvMb0BEokUWx4ZD"
  val appId = "1857972197747674" //"165032290796473"
  val appSecret = "8cdfbf7478ca2484368a819a314b4d1b" //"04e15624ccaa918c5b0c94ff7ed6b638"
  val accountId = "126959897820002"

  val adc =
    """{
      |  "fb_info": {
      |    "access_token": "EAAaZA0NzZAa9oBAEBuGaHYLGBhI8sWylZCy5V9BW8FoZAu2g73j5Pn2a5zuid9Ujbv3qo3fmphn8f5jsYGYqQ8CR8CtdFf7I8uEXpqzXWNX2MsKBAcivvH4TAHWhEmKkkzlWA1uiCXfZBcZB2bZCyFvMb0BEokUWx4ZD",
      |    "app_secret": "8cdfbf7478ca2484368a819a314b4d1b",
      |    "account_id": "126959897820002"
      |  },
      |  "name": "Swapnesh's test ad creative",
      |  "object_story_spec": {
      |    "page_id": "1772494609456362",
      |    "link_data": {
      |      "picture_url": "https://zp-uw2-nwreporting.s3.us-west-2.amazonaws.com/COW_20180401T223949779Z.jpg",
      |      "link": "www.zenreach.com",
      |      "message": "This offer Rocks"
      |    }
      |  }
      |}""".stripMargin
  val ts =
    """
      |[
      |	{
      | "targeting_type": "ZR_SEED",
      |		"age_max": 60,
      |		"age_min": 21,
      |		"geo_locations": {
      |			"location_types": ["home", "recent"],
      |			"custom_locations": [{
      |       "address_string": "920 Bay Street",
      |				"radius": "5",
      |				"country": "US",
      |				"latitude": "33.4657282",
      |				"longitude": "-112.0712293",
      |				"distance_unit": "mile"
      |			}]
      |		},
      |		"custom_audiences": [
      |			"23842790516120654"
      |		],
      |		"device_platforms": ["VALUE_MOBILE", "VALUE_DESKTOP"],
      |		"facebook_positions": ["feed", "instant_article"],
      |		"instagram_positions": ["stream"],
      |		"publisher_platforms": ["facebook"]
      |	},
      |	{
      | "targeting_type": "FB_CONTROL",
      |		"age_max": 60,
      |		"age_min": 21,
      |		"geo_locations": {
      |			"location_types": ["home", "recent"],
      |			"custom_locations": [{
      |     "address_string": "920 Bay Street",
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
      |		"publisher_platforms": ["facebook"]
      |	},
      |	{
      | "targeting_type": "FB_IG_CONTROL",
      |		"age_max": 60,
      |		"age_min": 21,
      |		"geo_locations": {
      |			"location_types": ["home", "recent"],
      |			"custom_locations": [{
      |     "address_string": "920 Bay Street",
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
      |		"publisher_platforms": ["facebook"]
      |	}
      |]
    """.stripMargin
}

//class CampaignService extends FacebookConversions {
//
//  def getTargetingSpec(specJson: String): immutable.Seq[TargetingSpec] = {
//    implicit val formats = DefaultFormats
//    parse(specJson).extract[List[TargetingSpec]]
//
//  }
//
//  def createSingleAdSetFacebook(account: AdAccount,
//                                campaignId: String,
//                                targetingSpec: TargetingSpec): AdSet = {
//    account
//      .createAdSet()
//      .setName(s"Adset ${targetingSpec.targeting_type} for $campaignId")
//      .setCampaignId(campaignId)
//      .setStatus(AdSet.EnumStatus.VALUE_PAUSED)
//      .setBillingEvent(AdSet.EnumBillingEvent.VALUE_IMPRESSIONS)
//      .setOptimizationGoal(AdSet.EnumOptimizationGoal.VALUE_IMPRESSIONS)
//      .setTargeting(targetingSpec.as[Targeting])
//      .execute()
//  }
//
//  def createAdSetsFacebook(account: AdAccount,
//                           campaignId: String,
//                           targetingSpec: Seq[TargetingSpec]): Seq[AdSet] =
//    for (ts <- targetingSpec)
//      yield createSingleAdSetFacebook(account, campaignId, ts)
//
//  def createCampaign(): Future[Campaign] = {
//    val context = new APIContext(FacebookParams.accessToken,
//                                 FacebookParams.appSecret).enableDebug(true)
//    val account: AdAccount = new AdAccount(FacebookParams.accountId, context)
//    val tSpec = getTargetingSpec(FacebookParams.ts)
//
//    Future {
//      account
//        .createCampaign()
//        .setName("Swapnesh's scala campaign2")
//        .setStatus(Campaign.EnumStatus.VALUE_PAUSED)
//        .setObjective(Campaign.EnumObjective.VALUE_POST_ENGAGEMENT)
//        .execute()
//
//    }
//  }
//
//  def getCampaign(): Future[Option[Campaign]] = ???
//
//}

object FacebookCampaignCreator extends FacebookConversions {

  def getTargetingSpec(specJson: String): immutable.Seq[TargetingSpec] = {
    implicit val formats = DefaultFormats
    parse(specJson).extract[List[TargetingSpec]]

  }

  def createAdSet(campaign: Campaign,
                  account: AdAccount,
                  targetingSpec: TargetingSpec,
                  adSetName: String,
                  dailyBudget: Long): AdSet = {
    val targeting = targetingSpec.as[Targeting]
    account
      .createAdSet()
      .setName(adSetName)
      .setCampaignId(campaign.getFieldId())
      .setStatus(AdSet.EnumStatus.VALUE_PAUSED)
      .setBillingEvent(AdSet.EnumBillingEvent.VALUE_IMPRESSIONS)
      .setOptimizationGoal(AdSet.EnumOptimizationGoal.VALUE_IMPRESSIONS)
      .setTargeting(targeting)
      .setIsAutobid(true)
      .setDailyBudget(dailyBudget)
      .execute()
  }

  def createAdCreative(fbInfo: FacebookAccount,
                       message: String,
                       pictureUrl: String,
                       linkUrl: String,
                       pageId: String): AdCreative = {

    val adAccount = createFacebookAccount(fbInfo.access_token,
                                          fbInfo.app_secret,
                                          fbInfo.account_id)

    adAccount
      .createAdCreative()
      .setObjectStorySpec(
        new AdCreativeObjectStorySpec()
          .setFieldLinkData(
            new AdCreativeLinkData()
              .setFieldPicture(pictureUrl)
              .setFieldLink(linkUrl)
              .setFieldMessage(message)
          )
          .setFieldPageId(pageId)
      )
      .execute()
  }

  def createAd(adAccount: AdAccount,
               adSet: AdSet,
               adCreative: AdCreative,
               adName: String) = {
    adAccount
      .createAd()
      .setName(adName)
      .setAdsetId(adSet.getId)
      .setCreative(adCreative)
      .setStatus("PAUSED")
      .execute()
  }

  def createFacebookAccount(accessToken: String,
                            appSecret: String,
                            accountId: String): AdAccount = {
    val context = new APIContext(accessToken, appSecret).enableDebug(true)
    new AdAccount(accountId, context)
  }

  def createCampaign(adAccount: AdAccount, campaignName: String): Campaign =
    adAccount
      .createCampaign()
      .setName(campaignName)
      .setStatus(Campaign.EnumStatus.VALUE_PAUSED)
      .setObjective(Campaign.EnumObjective.VALUE_POST_ENGAGEMENT)
      .execute()

  def getCRMBusinessName(locationId: String): String = ???

}

/*
import com.zenreach.nwservices.TestTargeting
import com.zenreach.nwservices.FacebookParams
import com.facebook.ads.sdk.Targeting
import com.facebook.ads.sdk._
import com.zenreach.nwservices.campaigns.entities.{
  FacebookConversions,
  TargetingSpec
}
import com.twitter.bijection.Conversion._

val ts = TestTargeting.getTargetingSpec(FacebookParams.ts)(0)
val acc = TestTargeting.createFacebookAccount(FacebookParams.accessToken, FacebookParams.appSecret, FacebookParams.accountId)
val c = TestTargeting.createCampaign(acc, "Swapnesh scala campaign")


val ac = TestTargeting.createAdCreative(acc, "Swapnesh's test ad creative", "This rocks", "https://zp-uw2-nwreporting.s3.us-west-2.amazonaws.com/COW_20180401T223949779Z.jpg", "www.zenreach.com", "1772494609456362")
val as = TestTargeting.createAdSet(c, acc, ts, "Swapnesh's test adset", 1000L)
TestTargeting.createAd(acc, as, ac, "Daniel Compton Pizza Campaign")
TestTargeting.createAdCreative(
object A extends FacebookConversions{
  def t(ts: TargetingSpec) = ts.as[Targeting]
}
// page id: 2016086572014097
// https://zs-uw2-nwreporting.s3.us-west-2.amazonaws.com/2i91zom1bsm01_20180330T174444432Z.jpg


 */
