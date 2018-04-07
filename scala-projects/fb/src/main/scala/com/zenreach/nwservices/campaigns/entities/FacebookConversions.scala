package com.zenreach.nwservices.campaigns.entities


import com.facebook.ads.sdk.Targeting.EnumDevicePlatforms
import com.twitter.bijection.{Injection, InversionFailure}
import com.facebook.ads.sdk._
import com.twitter.bijection.Conversion._

import scala.collection.JavaConverters._

trait FacebookConversions {
  implicit def zrCustomLocationToFbCustomLocation(
      customLocation: CustomLocation): TargetingGeoLocationCustomLocation = {
    new TargetingGeoLocationCustomLocation()
      .setFieldRadius(customLocation.radius.toDouble)
      .setFieldCountry(customLocation.country)
      .setFieldLatitude(customLocation.latitude.toDouble)
      .setFieldLongitude(customLocation.longitude.toDouble)
      .setFieldAddressString(customLocation.address_string)
  }

  implicit val zrCustomLocToFBCustomLoc
    : Injection[CustomLocation, TargetingGeoLocationCustomLocation] =
    Injection.build[CustomLocation, TargetingGeoLocationCustomLocation](
      zrCustomLocationToFbCustomLocation)(
      InversionFailure.failedAttempt
    )

  implicit def zrGeoLocationToFBGeoLocation(
      geoLocation: GeoLocation): TargetingGeoLocation = {

    val customLocations =
      geoLocation.custom_locations.map(_.as[TargetingGeoLocationCustomLocation])

    new TargetingGeoLocation()
      .setFieldLocationTypes(geoLocation.location_types.asJava)
      .setFieldCustomLocations(customLocations.asJava)

  }

  implicit val zrGeoLocToFBGeoLoc
    : Injection[GeoLocation, TargetingGeoLocation] =
    Injection.build[GeoLocation, TargetingGeoLocation](
      zrGeoLocationToFBGeoLocation)(
      InversionFailure.failedAttempt
    )

  implicit def zrTargetingSpecToFBTargetingSpec(
      targetingSpec: TargetingSpec): Targeting = {
    new Targeting()
      .setFieldAgeMin(targetingSpec.age_min)
      .setFieldAgeMax(targetingSpec.age_max)
      .setFieldDevicePlatforms(targetingSpec.device_platforms
        .map(EnumDevicePlatforms.valueOf _)
        .asJava)
      .setFieldFacebookPositions(targetingSpec.facebook_positions.asJava)
      .setFieldInstagramPositions(targetingSpec.instagram_positions.asJava)
      .setFieldPublisherPlatforms(targetingSpec.publisher_platforms.asJava)
      .setFieldGeoLocations(targetingSpec.geo_locations
        .as[TargetingGeoLocation])
      .setFieldCustomAudiences(
        targetingSpec.custom_audiences.map(new IDName().setFieldId(_)).asJava)
  }

  implicit val zrTargetingSpToFBTargetingSp
    : Injection[TargetingSpec, Targeting] =
    Injection.build[TargetingSpec, Targeting](zrTargetingSpecToFBTargetingSpec)(
      InversionFailure.failedAttempt
    )
}

object FacebookConversions extends FacebookConversions
