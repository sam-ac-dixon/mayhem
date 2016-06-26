var LibraryWebSockets = {
	$webSocketInstances: [],

	SocketCreate: function(url)
	{
		var str = Pointer_stringify(url);
		var socket = {
			socket: new WebSocket(str),
			error: null,
			messages: []
		}

		socket.socket.binaryType = 'arraybuffer';

		socket.socket.onmessage = function (e) {
			if (e.data instanceof Blob)
			{
				console.log("GOT BLOB DATA");
				var reader = new FileReader();
				reader.addEventListener("loadend", function() {
					var array = new Uint8Array(reader.result);
					socket.messages.push(array);
				});
				reader.readAsArrayBuffer(e.data);
			}
			else if (e.data instanceof ArrayBuffer)
			{
				console.log("GOT ARRAY DATA");
				var array = new Uint8Array(e.data);
				socket.messages.push(array);
			}
			else if (typeof (e.data) === "string")
			{
				socket.messages.push(e.data);
			}
		};

		socket.socket.onclose = function (e) {
			if (e.code != 1000)
			{
				if (e.reason != null && e.reason.length > 0)
					socket.error = e.reason;
				else
				{
					switch (e.code)
					{
						case 1001: 
							socket.error = "Endpoint going away.";
							break;
						case 1002: 
							socket.error = "Protocol error.";
							break;
						case 1003: 
							socket.error = "Unsupported message.";
							break;
						case 1005: 
							socket.error = "No status.";
							break;
						case 1006: 
							socket.error = "Abnormal disconnection.";
							break;
						case 1009: 
							socket.error = "Data frame too large.";
							break;
						default:
							socket.error = "Error "+e.code;
					}
				}
			}
		}
		var instance = webSocketInstances.push(socket) - 1;
		return instance;
	},

	SocketState: function (socketInstance)
	{
		var socket = webSocketInstances[socketInstance];
		return socket.socket.readyState;
	},

	SocketError: function (socketInstance, ptr, bufsize)
	{
	 	var socket = webSocketInstances[socketInstance];
	 	if (socket.error == null)
	 		return 0;
	    var str = socket.error.slice(0, Math.max(0, bufsize - 1));
	    writeStringToMemory(str, ptr, false);
		return 1;
	},

	SocketSend: function (socketInstance, ptr, length)
	{
		var socket = webSocketInstances[socketInstance];
		socket.socket.send (HEAPU8.buffer.slice(ptr, ptr+length));
	},

	AllMessageLength: function(socketInstance)
	{
		var socket = webSocketInstances[socketInstance];
		var count = 0;

		if (socket.messages.length == 0) {
			return count;
		}

		for (var i = 0; i < socket.messages.length; i++) {
			count += socket.messages[i].length;
		}

		return count;
	},

	NextMessageLength: function(socketInstance)
	{
		var socket = webSocketInstances[socketInstance];

		if (socket.messages.length == 0) {
			return 0;
		}

		return socket.messages[0].length;
	},

	SocketRecv: function (socketInstance, ptr)
	{
		var socket = webSocketInstances[socketInstance];
		if (socket.messages.length == 0){
			return 0;	
		}
		var str = socket.messages[0]
		socket.messages = socket.messages.slice(1);
	    writeStringToMemory(str, ptr, false);
	},

	SocketClose: function (socketInstance)
	{
		var socket = webSocketInstances[socketInstance];
		socket.socket.close();
	}
};

autoAddDeps(LibraryWebSockets, '$webSocketInstances');
mergeInto(LibraryManager.library, LibraryWebSockets);
