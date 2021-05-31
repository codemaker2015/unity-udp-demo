# UDP Server Client in Unity 3D and Node.js

Here is a quick tutorial on setting up a UDP server and client in Unity 3D and Node.js. For all things UDP in Node.js, you will need to use the dgram library, so read it up well and good.

### UDPReceive 

```cs

using UnityEngine;
using System.Collections;
 
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
 
public class UDPReceive : MonoBehaviour {
   
    // receiving Thread
    Thread receiveThread;
 
    // udpclient object
    UdpClient client;
 
    // public
    // public string IP = "127.0.0.1"; default local
    public int port; // define > init
 
    // infos
    public string lastReceivedUDPPacket="";
    public string allReceivedUDPPackets=""; // clean up this from time to time!
   
   
    // start from shell
    private static void Main()
    {
       UDPReceive receiveObj=new UDPReceive();
       receiveObj.init();
 
        string text="";
        do
        {
             text = Console.ReadLine();
        }
        while(!text.Equals("exit"));
    }
    // start from unity3d
    public void Start()
    {
        init();
    }
   
    // OnGUI
    void OnGUI()
    {
        Rect rectObj=new Rect(40,10,200,400);
            GUIStyle style = new GUIStyle();
                style.alignment = TextAnchor.UpperLeft;
        GUI.Box(rectObj,"# UDPReceive\n127.0.0.1 "+port+" #\n"
                    + "shell> nc -u 127.0.0.1 : "+port+" \n"
                    + "\nLast Packet: \n"+ lastReceivedUDPPacket
                    + "\n\nAll Messages: \n"+allReceivedUDPPackets
                ,style);
    }
       
    // init
    private void init()
    {
        print("UDPSend.init()");
        port = 5009;

        print("Sending to 127.0.0.1 : "+port);
        print("Test-Sending to this Port: nc -u 127.0.0.1  "+port+"");
 
        receiveThread = new Thread(
            new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
 
    }
 
    // receive thread
    private  void ReceiveData()
    {
        client = new UdpClient(port);
        while (true)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = client.Receive(ref anyIP);
                string text = Encoding.UTF8.GetString(data);
                print("Server: " + text);
                lastReceivedUDPPacket=text;
                allReceivedUDPPackets=allReceivedUDPPackets+text;
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }
   
    public string getLatestUDPPacket()
    {
        allReceivedUDPPackets="";
        return lastReceivedUDPPacket;
    }
}

```

### UDPSend

```cs

using UnityEngine;
using System.Collections;
 
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
 
public class UDPSend : MonoBehaviour
{
    private static int localPort;
    private string IP;  // define in init
    public int port;  // define in init
    IPEndPoint remoteEndPoint;
    UdpClient client;
    string strMessage="";
   
    private static void Main()
    {
        UDPSend sendObj=new UDPSend();
        sendObj.init();
        sendObj.sendEndless(" endless infos \n");
    }
    
    // start from unity3d
    public void Start()
    {
        init();
    }
   
    // OnGUI
    void OnGUI()
    {
        Rect rectObj=new Rect(40,380,200,400);
            GUIStyle style = new GUIStyle();
                style.alignment = TextAnchor.UpperLeft;
        GUI.Box(rectObj,"# UDPSend-Data\n127.0.0.1 "+port+" #\n"
                    + "shell> nc -lu 127.0.0.1  "+port+" \n"
                ,style);
        strMessage=GUI.TextField(new Rect(40,420,140,20),strMessage);
        if (GUI.Button(new Rect(190,420,40,20),"send"))
        {
            sendString(strMessage+"\n");
        }      
    }
   
    // init
    public void init()
    {
        print("UDPSend.init()");
        IP="127.0.0.1";
        port=5009;
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);
        client = new UdpClient();
        print("Sending to "+IP+" : "+port);
        print("Testing: nc -lu "+IP+" : "+port);
   
    }
 
    private void inputFromConsole()
    {
        try
        {
            string text;
            do
            {
                text = Console.ReadLine();
                if (text != "")
                {
                    byte[] data = Encoding.UTF8.GetBytes(text);
                    client.Send(data, data.Length, remoteEndPoint);
                }
            } while (text != "");
        }
        catch (Exception err)
        {
            print(err.ToString());
        }
 
    }
 
    // sendData
    private void sendString(string message)
    {
        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            client.Send(data, data.Length, remoteEndPoint);
        }
        catch (Exception err)
        {
            print(err.ToString());
        }
    }
   

    private void sendEndless(string testStr)
    {
        do
        {
            sendString(testStr);
        }
        while(true);
    }
}
 

```

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


```


### Screenshots

![Screenshots](https://github.com/codemaker2015/unity-udp-demo/blob/master/Screenshots/Screenshot.png)

### Things to note

* client.send() requires a proper Node.js Buffer object, not a plain string or number.
* The second parameter 0, of client.send() is the offset in the buffer where the UDP packet starts.
* The third parameter message.length, is the number of bytes we want to send from the offset in the buffer. In our case, the offset is 0, and the length is message.length (16 bytes), which is quite tiny and the whole buffer can be sent in a single UDP packet. This might always not be the case. For large buffers, you will need to iterate over the buffer and send it in smaller chunks of UDP packets.
* Exceeding the allowed packet size will not result in any error. The packet will be silently dropped. That's just the nature of UDP.
* The err object in the callback function of client.send() is going to be only of the DNS lookup kind.
* Make sure the HOST / IP address is in conformance with the IP version you use, else your packets will not reach the destination.

There you go! A quick primer on getting started with UDP in Node.js."# udp-client-server" 
