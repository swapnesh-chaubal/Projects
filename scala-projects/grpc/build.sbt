name := "ScalaGRPC"

version := "1.0"

scalaVersion := "2.11.11"

PB.targets in Compile := Seq(
  scalapb.gen() -> (sourceManaged in Compile).value
)


