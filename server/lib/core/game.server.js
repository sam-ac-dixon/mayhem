var Client = require('./client.js');
var World = require('../game/world.js');

var GameServer = function() {
  this.clients = {};
  this.commands = [];
  this.world = new World();
  console.log("Game Server Intialised");
}

GameServer.prototype.startUpdateLoop = function(){
  setInterval(function(){
    this.update();
  }.bind(this), 1000);
}

GameServer.prototype.registerClient = function(client) {
  this.clients[client.id] = client;
  this.world.addPlayer(client.id);

  client.conn.on("text", function (message) {
        // Add some validation
        this.pushCommand(client.id, JSON.parse(message));       
  }.bind(this));

  client.conn.on("close", function(code, reason) {
        this.world.removePlayer(client.id);
        delete this.client[client.id];
        console.log("Connection closed")
  }.bind(this));
}

GameServer.prototype.pushCommand = function(clientId, command){
  this.commands.push({"clientId": clientId, "command": command})
}

GameServer.prototype.update = function() {  
  var numOfCommands = this.commands.length;
  var commandsForNextUpdate = this.commands.splice(0,numOfCommands);
  this.world.update(commandsForNextUpdate);
  // serverInstance.sendWorldState();
}

module.exports = GameServer;