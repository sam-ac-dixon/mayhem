// =============================================================================
//  An Entity in the world.
// =============================================================================
var Entity = function() {
  this.x = 0;
  this.speed = 2; // units/s
}

// Apply user's input to this entity.
Entity.prototype.applyInput = function(input) {
  this.x += input.press_time*this.speed;
}

module.exports = Entity;