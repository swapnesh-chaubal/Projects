var app = require('express')();
var http = require('http').Server(app);
var io = require('socket.io')(http);
var fs = require("fs")
var file = "chat.db";

if (fs.existsSync(file)) {
    fs.unlinkSync(file);
}

var sqlite3 = require('sqlite3').verbose();
var db = new sqlite3.Database(file);
 

createTables = function() {
       db.serialize(function() {
           db.run("create table messages (id integer primary key autoincrement, msg text, userName text, timestamp text);");
           db.run("create table users (id integer primary key autoincrement, name text);");
           console.log("created tables");
         });
    }

addMessage = function(messageText, userName) {
       db.serialize(function() {
           
           var query = 'insert into messages values (null,"' + messageText + '", "' + userName + '", "' + new Date() + '");';
           db.run(query);
           console.log("Run query: " + query);
         });
    }

getMessages = function(io){
  db.all("SELECT userName, timestamp, msg FROM messages", function(err, msgInfo) {
	    var returnText = "";
      for (var i = 0; i < msgInfo.length; i++) {
		    returnText += msgInfo[i]['userName'] + " (" + msgInfo[i]['timestamp'] + ") : " + msgInfo[i]['msg'];
      	returnText += '\n';
      };

      console.log(returnText);
      io.emit('message', returnText);
  })
}

app.get('/', function(req, res){
  res.sendFile(__dirname + '/chatIndex.html');
});

io.on('connection', function(socket){
  socket.on('message', function(chatMsg){
  	var tuple = chatMsg.split('&');
  	messageText= tuple[0].split('=')[1];
  	userName = tuple[1].split('=')[1];
    addMessage(messageText, userName);
    getMessages(io);
  });
});

createTables();

http.listen(3000, function(){
  console.log('listening on *:3000');
});
