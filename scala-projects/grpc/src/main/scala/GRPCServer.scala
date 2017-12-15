import com.swapnesh.hello.HelloWorld

object GRPCServer {
  def main(args: Array[String]) = {
    val h = HelloWorld("Scala ROcks")
    println(h.hello)
  }
}
