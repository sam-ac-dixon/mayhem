var ws = require("nodejs-websocket");
var ServerManager = require("./lib/core/server.manager.js");
var Client = require("./lib/core/client.js");

var serverManager = new ServerManager();

var wss = ws.createServer(function(conn) {

    var client = new Client(conn);
    conn.sendText(JSON.stringify( {id:client.id} ));

    serverManager.joinGame(client);    

    conn.on("close", function(code, reason) {
        console.log("Connection closed")
    });
    
}).listen(1234)