var Entity = require('./entity.js');


var Server = function() {
  // Connected clients and their entities.
  this.clients = [];
  this.entities = [];

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

Server.prototype.connectClient = function(client) {
  // Give the Client enough data to identify itself.
  client.server = this;
  client.entity_id = this.clients.length;
  this.clients.push(client);

  // Create a new Entity for this Client.
  var entity = new Entity();
  this.entities.push(entity);
  entity.entity_id = client.entity_id;

  // Set the initial state of the Entity (e.g. spawn point)
  entity.x = 5;
  console.log("Now there are " + this.clients.length + " clients");
}

Server.prototype.update = function(serverInstance) {
  console.log("Updating");
  serverInstance.processInputs();
  serverInstance.sendWorldState();
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