var UUID = require('node-uuid')

var Client = function(conn) {
  this.id = UUID();
  this.conn = conn;  
}

module.exports = Client;