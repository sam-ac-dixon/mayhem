var Player = require('./player.js');
var Client = require('./client.js');
var UUID = require('node-uuid')

var Server = function() {
  // Connected clients and their entities.
  this.clients = [];
  this.commands = [];

  // Last processed input for each client.
  this.last_processed_input = [];

  console.log("Server Intialised");
}

Server.prototype.startUpdateLoop = function(){
    var t = this;

    setInterval(function(){
      t.update(t);
    }, 1000);
}

Server.prototype.pushCommand = function(client, command){
  //{ 
  //  move: 0 | 1,
  //  rotate: amount,
  //  currentWeapon: ID,
  //  fire: 0 | 1
  //}

  this._commands.push({"client": client, "command": command})
}

Server.prototype.connectClient = function() {
  var client = new Client();
  var newPlayer = new Player();

  client.id = UUID();

  newPlayer.x = 5;

  client.connectedClient = newPlayer;

  this.clients.push(client);

  console.log("Client " + client.id + " is now connected");

  return client.id
}

Server.prototype.update = function(serverInstance) {
  console.log("Updating");
  //serverInstance.processInputs();
  //serverInstance.sendWorldState();
}

Server.prototype.processInputs = function() {
  // Process all pending messages from clients.
  while (true) {
    var message = undefined; //this.network.receive();
    if (!message) {
      break;
    }

    // Update the state of the entity, based on its input.
    // We just ignore inputs that don't look valid; this is what prevents
    // clients from cheating.
    if (this.validateInput(message)) {
      var id = message.entity_id;
      this.entities[id].applyInput(message);
      this.last_processed_input[id] = message.input_sequence_number;
    }
  }
}

// Check whether this input seems to be valid (e.g. "make sense" according
// to the physical rules of the World)
Server.prototype.validateInput = function(input) {
  if (Math.abs(input.press_time) > 1/40) {
    return false;
  }
  return true;
}

// Send the world state to all the connected clients.
Server.prototype.sendWorldState = function() {
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

module.exports = Server;