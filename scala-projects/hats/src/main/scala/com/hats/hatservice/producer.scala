import java.nio.ByteBuffer

import com.amazonaws.services.kinesis.producer.{KinesisProducer, UserRecordResult}
import com.google.common.util.concurrent.{Futures, ListenableFuture}
import org.apache.http.concurrent.FutureCallback

import scala.concurrent.{Future, Promise}

trait EventEmitter {
  def emitEvent(event: String)
}

//implicit class RichListenableFuture[T](lf: ListenableFuture[T]) {
//  def asScala: Future[T] = {
//    val p = Promise[T]()
//    Futures.addCallback(lf, new FutureCallback[T] {
//      def onFailure(t: Throwable): Unit = p failure t
//      def onSuccess(result: T): Unit    = p success result
//    })
//    p.future
//  }
//}

object EventEmitter extends EventEmitter {
  def producer = new KinesisProducer()

  override def emitEvent(event: String): ListenableFuture[UserRecordResult] = {
    val listenableFuture =
      producer.addUserRecord("testStream",
        "1",
        ByteBuffer.wrap(event.getBytes("UTF-8")))

    val p = Promise[UserRecordResult]
    Futures.addCallback(listenableFuture, new FutureCallback[UserRecordResult])

  }
}
