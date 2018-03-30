package com.zenreach.nwservices.campaigns.entities

import java.sql.Timestamp

case class Campaign(locationId: String,
                    startTime: Timestamp,
                    endTime: Timestamp,
                    imageUrl: String,
                    zrSeed: Boolean,
                    fBControl: Boolean,
                    fbigControl: Boolean,
                    created: Timestamp)

