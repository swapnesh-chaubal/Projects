import datetime
import time
import http.server
import subprocess
from command_sender import CommandSender

JSON_FORMAT = "{{'command': {cmd}, 'result': {output}}}"
DEFAULT_PORT = 9000
OUTPUT_PORT_GET_STRING = "output_port"
GET_IP_CMD = "hostname"

class RemoteServer():
    def __init__(self):
        self.cmdSender = CommandSender()

    def get_my_ip(self):
        """Returns this computers IP address as a string."""
        ip = subprocess.check_output(GET_IP_CMD, shell=True).decode('utf-8')[:-1]
        return ip.strip()

    def main(self):
        port = DEFAULT_PORT

        print("Starting HMAT Web Server at:\n\n"
              "\thttp://{addr}:{port}\n\n"
              .format(addr=self.get_my_ip(), port=port))

        # run the server
        server_address = ('', port)
        try:
            server = http.server.HTTPServer(server_address, RemoteWebHandler)
            server.serve_forever()
        except KeyboardInterrupt:
            print('^C received, shutting down server')
            server.socket.close()

    def handleCommand(self, query):
        return self.cmdSender.handleCommand(query)
    
remoteServer = RemoteServer()

class RemoteWebHandler(http.server.BaseHTTPRequestHandler):
    def do_POST(self):
        import pdb
        len = self.headers['Content-length']
        headers = self.headers
        print("headers: \n" + headers.__str__())
        commandStr = self.rfile.read(int(len)).decode("utf-8")
        remoteServer.handleCommand(commandStr)

        # remove stuff below
        #print("Packet: \n" + command_string)
        self.send_response(200)
        self.send_header("Content-type", "application/json")
        self.end_headers()
        try:
           # we check if this is the start of a new test and if it is, we initialize the log server
           result = remoteServer.handle_post(command_string)
           self.send_response(200)
           self.send_header("Content-type", "application/json")
           self.end_headers()
           if (result is not None):
               self.wfile.write(bytes(JSON_FORMAT.format(
                cmd=result[0],
                output=result[1],
              ), 'UTF-8'))
           else:
               self.wfile.write(bytes(JSON_FORMAT.format(
                 cmd=command_string,
                 output="Success",
               ), 'UTF-8'))
  
        except Exception as e:
           self.send_header("Content-type", "application/json")
           self.end_headers()
           self.wfile.write(bytes(JSON_FORMAT.format(
             cmd="Exception",
             output=str(e),
           ), 'UTF-8'))


if __name__ == "__main__":
    remoteServer.main() 
