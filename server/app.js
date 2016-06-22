var ws = require("nodejs-websocket");
var Server = require("./server.js");

var game_server = new Server();
game_server.startUpdateLoop();

var socket = ws.createServer(function(conn) {
    var newClientID = game_server.connectClient(conn);

    conn.sendText('{"id":"' + newClientID + '"}');
	
	conn.on("text", function(message){
        console.log("Recieved String: " + message);
    })
	
    conn.on("close", function(code, reason) {
        console.log("Connection closed")
    })
}).listen(8001)
