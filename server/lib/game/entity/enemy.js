var Enemy = function() {
  this.x = 10;
  this.y = 10;
  this.angle = 0;
  this.speed = 2; // units/s
  this.health = 100;

  this.width = 20;
  this.height = 20;

  this.damage = 20;
}

module.exports = Enemy;