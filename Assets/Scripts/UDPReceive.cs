 
/*
    -----------------------
    UDP-Receive (send to)
    -----------------------
    // > receive
    // 127.0.0.1 : 5009
   
    // send
    // nc -u 127.0.0.1 5009
*/
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