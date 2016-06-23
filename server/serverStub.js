var ws = require("nodejs-websocket");
var Server = require("./server.js");

var updateRateMS = 30;

var game_server = new Server();
//game_server.startUpdateLoop();

var data_payload = { 
  id: "abc",
  players: [{
  	id: "abc",
  	x: 0,
  	y: 0,
  	a: 0
  },
  {
  	id: "def",
  	x: 0,
  	y: 0,
  	a: 0
  }],
  bullets:[{
  	x: 0,
  	y: 0,
  	a: 0
  },
  {
  	x: 0,
  	y: 0,
  	a: 0
  }]
};

var socket = ws.createServer(function(conn) {
    var newClientID = game_server.connectClient(conn);
    var clientConnection = conn;

    setInterval(function() {
    	console.log("Running thingy");
    	data_payload["players"][0].x += 0.01;
    	data_payload["players"][1].x += 0.01;
    	console.log(JSON.stringify(data_payload));
    	clientConnection.sendText(JSON.stringify(data_payload));
    }, updateRateMS);

	conn.on("text", function(message){
        console.log("Recieved String: " + message);
    })
	
    conn.on("close", function(code, reason) {
        console.log("Connection closed")
    })
}).listen(8001)