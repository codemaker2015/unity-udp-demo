var udp = require('dgram');

// --------------------creating a udp server --------------------

// // creating a udp server
// var server = udp.createSocket('udp4');
// var fs = require('fs');

// // emits when any error occurs
// server.on('error', function (error) {
//   console.log('Error: ' + error);
//   server.close();
// });

// // emits on new datagram msg
// server.on("message", function (msg, info) {
//   console.log("Message: ", msg)
//   var stream = fs.createWriteStream("udp-stream.log", { 'flags': 'a' });
//   stream.once('open', function (fd) {
//     stream.write(msg + "\r\n");
//   });

//   //sending msg
//   server.send(msg, info.port, 'localhost', function (error) {
//     if (error) {
//       client.close();
//     } else {
//       console.log('Data sent !!!');
//     }
//   });

// });

// //emits when socket is ready and listening for datagram msgs
// server.on('listening', function () {
//   var address = server.address();
//   var port = address.port;
//   var ipaddr = address.address;
//   console.log(ipaddr + ' server is listening at port ' + port);
// });

// //emits after the socket is closed using socket.close();
// server.on('close', function () {
//   console.log('Socket is closed !');
// });

// server.bind(5009);

// setTimeout(function(){
// server.close();
// },8000);

// // -------------------- udp client ----------------

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
client.send(data,5009,'localhost',function(error){
  if(error){
    client.close();
  }else{
    console.log('Client: ', data.toString());
  }
});

var data1 = Buffer.from('hello');
var data2 = Buffer.from('world');

//sending multiple msg
client.send([data1,data2],5009,'localhost',function(error){
  if(error){
    client.close();
  }else{
    console.log('Client: ', data1.toString(), data2.toString());
  }
});
