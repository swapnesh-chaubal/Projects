package com.zenreach.nwservices

import com.twitter.app.Flag
import com.twitter.conversions.time
import com.twitter.finagle.{Http, Service}
import com.twitter.finagle.http.{Request, Response}
import com.twitter.finagle.param.Stats
import com.twitter.server.TwitterServer
import com.twitter.util.{Await, Future}
import io.finch._
import io.finch.circe._
import io.finch.syntax._
import io.circe.generic.auto._
import com.twitter.util.Future
import com.zenreach.nwservices.campaigns.entities.TargetingSpec

case class Message(message: String)

case class FullName(firstName: String, lastName: String)

class ExampleService {
  val message = Message("Hello, world!")

  def getMessage(): Future[Message] = {
    Future.value(message)
  }

  def getFullName(): Future[FullName] =
    Future.value(FullName("Swapnesh", "Chaubal"))

  def acceptMessage(incomingMessage: Seq[TargetingSpec]): Future[Seq[TargetingSpec]] =
    Future.value(incomingMessage)

}

object CampainCreation extends TwitterServer {
  val port: Flag[Int] = flag("port", 8081, "TCP port for HTTP server")

  val exampleService = new ExampleService

  def hello: Endpoint[Message] = get("hello") {
    exampleService.getMessage().map(Ok)
  }

  def fullName: Endpoint[FullName] = get("fullName") {
    exampleService.getFullName().map(Ok)
  }

  def acceptedMessage: Endpoint[Seq[TargetingSpec]] = jsonBody[Seq[TargetingSpec]]

  def accept: Endpoint[Seq[TargetingSpec]] = post("accept" :: acceptedMessage) {
    incomingMessage: Seq[TargetingSpec] =>
      exampleService.acceptMessage(incomingMessage).map(Ok)
  }

  val api = (hello :+: fullName :+: accept).handle {
    case e: Exception => InternalServerError(e)
  }

  def main(): Unit = {

    val server = Http.server
      .withStatsReceiver(statsReceiver)
      .serve(s":${port()}", api.toServiceAs[Application.Json])

    closeOnExit(server)

    Await.ready(adminHttpServer)
  }
}
