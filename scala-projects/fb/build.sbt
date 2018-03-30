name := "hello"

version := "1.0"

scalaVersion := "2.11.1"

libraryDependencies ++= Seq(
    "com.facebook.ads.sdk" % "facebook-java-ads-sdk" % "2.11.1",
    "org.json4s" %% "json4s-ext" % "3.6.0-M2",
    "org.json4s" %% "json4s-jackson" % "3.6.0-M2",
    "com.twitter" %% "bijection-core" % "0.9.5",
    "com.twitter" %% "bijection-util" % "0.9.5"
)
