require('./entity.js')
require('./socketNetwork.js')
// =============================================================================
//  The Client.
// =============================================================================
var Client = function() {
  this.entity = null;

  // Input state.
  this.key_left = false;
  this.key_right = false;

  // Simulated network connection.
  this.network = new Network();
  this.server = null;

  // Unique ID of our entity. Assigned by Server on connection.
  this.entity_id = null;

  // Data needed for reconciliation.
  this.input_sequence_number = 0;
  this.pending_inputs = [];
}


// Update Client state.
Client.prototype.update = function() {
  // Listen to the server.
  this.processServerMessages();

  if (this.entity == null) {
    return;  // Not connected yet.
  }

  // Process inputs.
  this.processInputs();

  // Render the World.
  renderWorld(player_canvas, [this.entity]);

  // Show some info.
  var info = "Non-acknowledged inputs: " + this.pending_inputs.length;
  player_status.textContent = info;
}

// Process all messages from the server, i.e. world updates.
// If enabled, do server reconciliation.
Client.prototype.processServerMessages = function() {
  while (true) {
    var message = this.network.receive();
    if (!message) {
      break;
    }

    // World state is a list of entity states.
    for (var i = 0; i < message.length; i++) {
      var state = message[i];

      if (state.entity_id == this.entity_id) {
        // Got the position of this client's entity.

        if (!this.entity) {
          // If this is the first server update, create a local entity.
          this.entity = new Entity();
        }

        // Set the position sent by the server.
        this.entity.x = state.position;

        if (server_reconciliation) {
          // Server Reconciliation. Re-apply all the inputs not yet processed by
          // the server.
          var j = 0;
          while (j < this.pending_inputs.length) {
            var input = this.pending_inputs[j];
            if (input.input_sequence_number <= state.last_processed_input) {
              // Already processed. Its effect is already taken into account
              // into the world update we just got, so we can drop it.
              this.pending_inputs.splice(j, 1);
            } else {
              // Not processed by the server yet. Re-apply it.
              this.entity.applyInput(input);
              j++;
            }
          }
        } else {
          // Reconciliation is disabled, so drop all the saved inputs.
          this.pending_inputs = [];
        }
      } else {
        // TO DO: add support for rendering other entities.
      }
    }
  }
}


// Get inputs and send them to the server.
// If enabled, do client-side prediction.
Client.prototype.processInputs = function() {
  // Compute delta time since last update.
  var now_ts = +new Date();
  var last_ts = this.last_ts || now_ts;
  var dt_sec = (now_ts - last_ts) / 1000.0;
  this.last_ts = now_ts;

  // Package player's input.
  var input;
  if (this.key_right) {
    input = { press_time: dt_sec };
  } else if (this.key_left) {
    input = { press_time: -dt_sec };
  } else {
    // Nothing interesting happened.
    return;
  }

  // Send the input to the server.
  input.input_sequence_number = this.input_sequence_number++;
  input.entity_id = this.entity_id;
  this.server.network.send(client_server_lag, input);

  // Do client-side prediction.
  if (client_side_prediction) {
    this.entity.applyInput(input);
  }

  // Save this input for later reconciliation.
  this.pending_inputs.push(input);
}