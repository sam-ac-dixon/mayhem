var Player = function() {
  this.x = 10;
  this.y = 10;
  this.angle = 0;
  this.speed = 2; // units/s
  this.score = 0;

  this.health = 100;

  this.width = 20;
  this.height = 20;
}

// Apply user's input to this entity.
Player.prototype.applyInput = function(input) {
  // this.x += input.press_time*this.speed;
  this.x += 1;
}

module.exports = Player;