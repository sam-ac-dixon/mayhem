var ws = require("nodejs-websocket");
var Server = require("./server.js");

var game_server = new Server();
game_server.startUpdateLoop();

var socket = ws.createServer(function(conn) {
    var newClientID = game_server.connectClient(conn);

    conn.sendText("id:" + newClientID);

    conn.on("binary", function(inStream) {
        // Empty buffer for collecting binary data
        var data = new Buffer(0)
            // Read chunks of binary data and add to the buffer
        inStream.on("readable", function() {
            var newData = inStream.read()
            if (newData)
                data = Buffer.concat([data, newData], data.length + newData.length)
        })
        inStream.on("end", function() {
            game_server.pushCommand(conn, data.toString('utf8'))
        })
    })
    conn.on("close", function(code, reason) {
        console.log("Connection closed")
    })
}).listen(8001)
