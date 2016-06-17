
var io = require('socket.io');
var port = 1234;

// Create a Socket.IO instance, passing it our server
var socket = io.listen(port);

// Add a connect listener
socket.on('connection', function(client){ 
    console.log('Connection to client established');

    // Success!  Now listen to messages to be received
    client.on('message',function(event){ 
        console.log('Received message from client!',event);
    });

    // client.on('disconnect',function(){
    //     clearInterval(interval);
    //     console.log('Server has disconnected');
    // });
});

console.log('Server running at http://127.0.0.1:' + port + '/');