var Player = require('./entity/player.js')
var Enemy = require('./entity/enemy.js')
var QuadTree = require('./lib/quad.tree.js')


var World = function() {
  this.players = [];
  this.playerProjectiles = [];
  this.enemies = [];

  this.enemies.push(new Enemy());

  var bounds = {
    x:0,
    y:0,
    width:1000,
    height:1000
	};

  this.tree = new QuadTree(bounds, false, 10, 10);
}

World.prototype.addPlayer = function(clientId) {
	this.players[clientId] = new Player();
}

World.prototype.removePlayer = function(clientId) {
	delete this.players[clientId];
}

World.prototype.update = function(messages) {

	console.log("WORLD IS UPDATING");
	
	this.tree.clear();

	// update all positions
	this.updatePositions(messages);

	// resolve player collisions
	this.resolvePlayerCollisions();

	// resolve projectile collisions

	// resolve enemy attacks
	this.resolveEnemyAttacks();

}


World.prototype.updatePositions = function(messages) {
	
	// what happens when players join/leave during update?
	for (i = 0; i < messages.length; ++i) {
		var message = messages[i];
		var command = message.command;
		var player = this.players[message.clientId];

	    player.x += player.speed * Math.cos(command.angle * Math.PI / 180);
		player.y += player.speed * Math.sin(command.angle * Math.PI / 180);
	}

	// THIS IS UGLY - REFACTOR
	for (var clientId in this.players) {
	  if (this.players.hasOwnProperty(clientId)) {
	    this.tree.insert(this.players[clientId]);
	  }
	}

	for (i = 0; i < this.playerProjectiles.length; ++i) {
	    //projectile
	}
	this.tree.insert(this.playerProjectiles);

	for (i = 0; i < this.enemies.length; ++i) {
	    //moveEnemy
	}
	this.tree.insert(this.enemies);

}

World.prototype.resolvePlayerCollisions = function() {
	for (i = 0; i < this.players.length; ++i) {
	    // todo
	}
}

World.prototype.resolveEnemyAttacks = function() {
	for (i = 0; i < this.enemies.length; ++i) {
	    enemy = this.enemies[i];

		items = this.tree.retrieve(enemy);

		for(var j = 0; j < items.length; j++) {
			
			item = items[j];
			
			if(item instanceof Player) {
				if(isColliding(enemy, item)) {
					item.health -= enemy.damage;
				}
			}
		}
	}
}

function isColliding(a, b) {
  return (a.x <= (b.x + b.width) &&
          b.x <= (a.x + a.width) &&
          a.y <= (b.y + b.height) &&
          b.y <= (a.y + a.height))
}

module.exports = World;