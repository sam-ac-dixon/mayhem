var GameServer = require('./game.server.js');
var Client = require('./client.js');

var ServerManager = function() {
  this.gameServers = [];
  console.log("Server manager up and running!");
}

ServerManager.prototype.joinGame = function(client) { 
    console.log("Join requested by: " + client.id);
    
    if(this.gameServers.length == 0) {      
      var game = new GameServer();
      this.gameServers.push(game);
    }

    this.gameServers[0].register(client);
}

module.exports = ServerManager;