var Client = require('./client.js');

var GameServer = function() {
  this.clients = {};
  this.commands = [];
  console.log("Game Server Intialised");
}

GameServer.prototype.startUpdateLoop = function(){
    setInterval(function(){
      this.update();
    }, 1000);
}

GameServer.prototype.pushCommand = function(clientId, command){
  this.commands.push({"clientId": clientId, "command": command})
}

GameServer.prototype.register = function(client) {
  this.clients[client.id] = client;
  client.conn.on("text", function (message) {
        console.log("Game server received a message from: " + client.id);  
        // game_server.pushCommand(clientId, JSON.parse(message));       
  });
}

GameServer.prototype.update = function() {
  console.log("Updating");
  // serverInstance.processInputs();
  // serverInstance.sendWorldState();
}

GameServer.prototype.processInputs = function() {
  // Process all pending messages from clients.
  var numOfCommands = this.commands.length;

  for (i = 0; i < numOfCommands; i++) { 
    var userCommand = this.commands[i];
    var client = this.clients[userCommand["clientId"]];
    client.connectedPlayer.applyInput(userCommand["command"]);
  }

  this.commands.splice(0,numOfCommands);
}

// Send the world state to all the connected clients.
GameServer.prototype.sendWorldState = function() {
  // Gather the state of the world. In a real app, state could be filtered to
  // avoid leaking data (e.g. position of invisible enemies).
  var world_state = [];
  var num_clients = this.clients.length;
  for (var i = 0; i < num_clients; i++) {
    var entity = this.entities[i];
    world_state.push({entity_id: entity.entity_id,
                      position: entity.x,
                      last_processed_input: this.last_processed_input[i]});
  }

  // Broadcast the state to all the clients.
  for (var i = 0; i < num_clients; i++) {
    var client = this.clients[i];
    console.log(world_state);
    //client.network.send(client_server_lag, world_state);
  }
}

module.exports = GameServer;