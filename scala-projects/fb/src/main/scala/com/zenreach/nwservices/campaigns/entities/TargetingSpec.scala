package com.zenreach.nwservices.campaigns.entities

case class CustomLocation(radius: String,
                          country: String,
                          latitude: String,
                          longitude: String,
                          distance_unit: String)

case class GeoLocation(countries: List[String],
                       location_types: List[String],
                       custom_locations: List[CustomLocation])

case class TargetingSpec(targeting_type: String,
                age_min: Long,
                age_max: Long,
                geo_locations: GeoLocation,
                custom_audiences: List[String],
                device_platforms: List[String],
                facebook_positions: List[String],
                instagram_positions: List[String],
                publisher_platforms: List[String])