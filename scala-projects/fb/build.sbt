name := "campaigns-service"

version := "1.0"

scalaVersion := "2.11.1"

lazy val json4sVersion = "3.5.3"

// Facebook SDK
libraryDependencies ++= Seq(
  "com.facebook.ads.sdk" % "facebook-java-ads-sdk" % "2.11.1"
)

// Json conversions
libraryDependencies ++= Seq(
  "org.json4s" %% "json4s-ext" % json4sVersion,
  "org.json4s" %% "json4s-jackson" % json4sVersion,
  "com.fasterxml.jackson.module" %% "jackson-module-scala" % "2.9.4"
//  "org.json4s" %% "json4s-ext" % "3.6.0-M2",
//  "org.json4s" %% "json4s-jackson" % "3.6.0-M2"
)

// Twitter libs for bijection/conversions
libraryDependencies ++= Seq(
  "com.twitter" %% "bijection-core" % "0.9.5",
  "com.twitter" %% "bijection-util" % "0.9.5"
)

// finch for web services
libraryDependencies ++= Seq(
    "com.twitter" %% "twitter-server" % "18.3.0",
    "com.github.finagle" %% "finch-core" % "0.17.0",
    "com.github.finagle" %% "finch-circe" % "0.17.0",
    "io.circe" %% "circe-generic" % "0.9.0"
)