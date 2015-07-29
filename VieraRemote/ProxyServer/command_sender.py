
import http.client
import json

class CommandSender():
    def __init__(self):
        self.commands = set()
        self.setCommands()
        self.tvServer = "com-mid1"
        self.tvPort = 55000
        self.headers = {"Accept": "text/xml",
                        "Cache-Control": "no-cache",
                        "Pragma": "no-cache",
                        'SOAPACTION': '"urn:panasonic-com:service:p00NetworkControl:1#X_SendKey"',
                        #"Content-Length": "317",
                        "Content-Type": 'text/xml;charset="utf-8"',
                        }

    def handleCommand(self, commandStr):
       commands = json.loads(commandStr)
       if("action" not in commands.keys()):
           return false

       key = commands["action"]
       if(key in self.commands):
           #body = self.getRequestBody()
           body = '<?xml version="1.0" encoding="utf-8"?><s:Envelope xmlns:s="http://schemas.xmlsoap.org/soap/envelope/" s:encodingStyle="http://schemas.xmlsoap.org/soap/encoding/"><s:Body><u:X_SendKey xmlns:u="urn:panasonic-com:service:p00NetworkControl:1"><X_KeyEvent>{}</X_KeyEvent></u:X_SendKey></s:Body></s:Envelope>'.format(key)

           self.sendCommand(self.headers, body)


    def sendCommand(self, headers, body):
       #import pdb
       #pdb.set_trace()
       conn = http.client.HTTPConnection(self.tvServer, self.tvPort)
       conn.request("POST", "/nrc/control_0", body, headers)
       response = conn.getresponse()

    def setCommands(self):
        self.commands = set()
        self.commands.add("NRC_CH_DOWN-ONOFF") # channel down
        self.commands.add("NRC_CH_UP-ONOFF") # channel up
        self.commands.add("NRC_VOLUP-ONOFF") # volume up
        self.commands.add("NRC_VOLDOWN-ONOFF") # volume down
        self.commands.add("NRC_MUTE-ONOFF") # mute
        self.commands.add("NRC_TV-ONOFF") # TV
        self.commands.add("NRC_CHG_INPUT-ONOFF") # AV,
        self.commands.add("NRC_RED-ONOFF") # red
        self.commands.add("NRC_GREEN-ONOFF") # green
        self.commands.add("NRC_YELLOW-ONOFF") # yellow
        self.commands.add("NRC_BLUE-ONOFF") # blue
        self.commands.add("NRC_VTOOLS-ONOFF") # VIERA tools
        self.commands.add("NRC_CANCEL-ONOFF") # Cancel / Exit
        self.commands.add("NRC_SUBMENU-ONOFF") # Option
        self.commands.add("NRC_RETURN-ONOFF") # Return
        self.commands.add("NRC_ENTER-ONOFF") # Control Center click / enter
        self.commands.add("NRC_RIGHT-ONOFF") # Control RIGHT 
        self.commands.add("NRC_LEFT-ONOFF") # Control LEFT
        self.commands.add("NRC_UP-ONOFF") # Control UP
        self.commands.add("NRC_DOWN-ONOFF") # Control DOWN
        self.commands.add("NRC_3D-ONOFF") # 3D button
        self.commands.add("NRC_SD_CARD-ONOFF") # SD-card
        self.commands.add("NRC_DISP_MODE-ONOFF") # Display mode / Aspect ratio
        self.commands.add("NRC_MENU-ONOFF") # Menu
        self.commands.add("NRC_INTERNET-ONOFF") # VIERA connect
        self.commands.add("NRC_VIERA_LINK-ONOFF") # VIERA link
        self.commands.add("NRC_EPG-ONOFF") # Guide / EPG
        self.commands.add("NRC_TEXT-ONOFF") # Text / TTV
        self.commands.add("NRC_STTL-ONOFF") # STTL / Subtitles
        self.commands.add("NRC_INFO-ONOFF") # info 
        self.commands.add("NRC_INDEX-ONOFF") # TTV index
        self.commands.add("NRC_HOLD-ONOFF") # TTV hold / image freeze
        self.commands.add("NRC_R_TUNE-ONOFF") # Last view
        self.commands.add("NRC_POWER-ONOFF") # Power off
        self.commands.add("NRC_REW-ONOFF") # rewind
        self.commands.add("NRC_PLAY-ONOFF") # play
        self.commands.add("NRC_FF-ONOFF") # fast forward
        self.commands.add("NRC_SKIP_PREV-ONOFF") # skip previous
        self.commands.add("NRC_PAUSE-ONOFF") # pause
        self.commands.add("NRC_SKIP_NEXT-ONOFF") # skip next
        self.commands.add("NRC_STOP-ONOFF") # stop
        self.commands.add("NRC_REC-ONOFF") # record

    def getRequestBody(self):      
        xmlBody = []
        xmlBody.append('<?xml version="1.0" encoding="utf-8"?>\n')
        xmlBody.append('<s:Envelope xmlns:s="http:#schemas.xmlsoap.org/soap/envelope/" s:encodingStyle="http://schemas.xmlsoap.org/soap/encoding/">\n')
        xmlBody.append('<s:Body>\n<u:X_SendKey xmlns:u="urn:panasonic-com:service:p00NetworkControl:1">\n')
        xmlBody.append('<X_KeyEvent>{}</X_KeyEvent>\n')
        xmlBody.append('</u:X_SendKey>\n</s:Body>\n</s:Envelope>')
        return ''.join(xmlBody)        
  




