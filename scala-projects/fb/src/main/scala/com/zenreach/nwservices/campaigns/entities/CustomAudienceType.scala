package com.zenreach.nwservices.campaigns.entities

sealed trait CustomAudienceType { def str: String }

case object ZRSeed extends CustomAudienceType { val str = "ZRSeed" }
case object FBControl extends CustomAudienceType { val str = "FBControl" }
case object FBIGControl extends CustomAudienceType { val str = "FBIGControl" }
case object NoType extends CustomAudienceType { val str = "NoType" }
