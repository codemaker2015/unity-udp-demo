# UDP Server Client in Node.js

Here is a quick tutorial on setting up a UDP server and client in Node.js. For all things UDP in Node.js, you will need to use the dgram library, so read it up well and good.

### UDP Server

Here is a simple example of a UDP server.

```js

var udp = require('dgram');

// --------------------creating a udp server --------------------

// creating a udp server
var server = udp.createSocket('udp4');
var fs = require('fs');

// emits when any error occurs
server.on('error',function(error){
  console.log('Error: ' + error);
  server.close();
});

// emits on new datagram msg
server.on("message", function (msg, info) {
  var stream = fs.createWriteStream("udp-stream.log", {'flags': 'a'});
  stream.once('open', function(fd) {
    stream.write(msg+"\r\n");
  });

//sending msg
server.send(msg,info.port,'localhost',function(error){
  if(error){
    client.close();
  }else{
    console.log('Data sent !!!');
  }
});

});

//emits when socket is ready and listening for datagram msgs
server.on('listening',function(){
  var address = server.address();
  var port = address.port;
  var family = address.family;
  var ipaddr = address.address;
  console.log('Server is listening at port' + port);
  console.log('Server ip :' + ipaddr);
  console.log('Server is IP4/IP6 : ' + family);
});

//emits after the socket is closed using socket.close();
server.on('close',function(){
  console.log('Socket is closed !');
});

server.bind(5009);

// setTimeout(function(){
// server.close();
// },8000);

```

### Things to note

HOST is optional in server.bind(). If omitted, it will be listening on 0.0.0.0, which might be what you want in some cases.
The message event is fired, when a UDP packet arrives destined for this server.
The listening event is fired, when the server has initialized and all ready to receive UDP packets. dgram.createSocket() can either accept 'udp4' or 'udp6'. The former uses IPv4, the later uses IPv6.

### UDP Client

And here is a simple UDP client.

```js

var buffer = require('buffer');

// creating a client socket
var client = udp.createSocket('udp4');

//buffer msg
var data = Buffer.from('codemaker');

client.on('message',function(msg,info){
  console.log('Data received from server : ' + msg.toString());
  console.log('Received %d bytes from %s:%d\n',msg.length, info.address, info.port);
});

//sending msg
client.send(data,2222,'localhost',function(error){
  if(error){
    client.close();
  }else{
    console.log('Data sent !!!');
  }
});

var data1 = Buffer.from('hello');
var data2 = Buffer.from('world');

//sending multiple msg
client.send([data1,data2],2222,'localhost',function(error){
  if(error){
    client.close();
  }else{
    console.log('Data sent !!!');
  }
});

```

### Things to note

* client.send() requires a proper Node.js Buffer object, not a plain string or number.
* The second parameter 0, of client.send() is the offset in the buffer where the UDP packet starts.
* The third parameter message.length, is the number of bytes we want to send from the offset in the buffer. In our case, the offset is 0, and the length is message.length (16 bytes), which is quite tiny and the whole buffer can be sent in a single UDP packet. This might always not be the case. For large buffers, you will need to iterate over the buffer and send it in smaller chunks of UDP packets.
* Exceeding the allowed packet size will not result in any error. The packet will be silently dropped. That's just the nature of UDP.
* The err object in the callback function of client.send() is going to be only of the DNS lookup kind.
* Make sure the HOST / IP address is in conformance with the IP version you use, else your packets will not reach the destination.

There you go! A quick primer on getting started with UDP in Node.js."# udp-client-server" 
