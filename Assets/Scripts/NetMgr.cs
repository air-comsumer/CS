using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class NetMgr : MonoBehaviour
{
    //try change
    public int x =1;
    private static NetMgr instance;
    public static NetMgr Instance
    {
        get
        {
            return instance;
        }
    }
    private void Awake()
    {
        instance = this;
    }
    public Socket socket;
    private Queue<string> msgQueue = new Queue<string>();
    private Queue<string> sendQueue = new Queue<string>();
    private byte[] result = new byte[1024*1024];
    private int resultNum;
    private bool isConnect;
    void Start()
    {
        
    }
    public void Connect(string IP,int port)
    {
        if(socket==null)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(IP), port);
        try
        {
            socket.Connect(endPoint);
            ThreadPool.QueueUserWorkItem(SendMessage);
            ThreadPool.QueueUserWorkItem(ReceiveMessage);
        }
        catch
        {
            Debug.Log("Á¬½ÓÊ§°Ü");
        }
    }
    public void Send(string message)
    {
        sendQueue.Enqueue(message);
    }
    private void SendMessage(object state)
    {
        while(isConnect)
        {
            if (sendQueue.Count > 0)
            {
                string message = sendQueue.Dequeue();
                byte[] data = Encoding.UTF8.GetBytes(message);
                socket.Send(data);
            }
        }
    }

    private void ReceiveMessage(object state)
    {
        while(isConnect)
        {
            if (socket.Available > 0)
            {
                resultNum = socket.Receive(result);
                string message = Encoding.UTF8.GetString(result, 0, resultNum);
                msgQueue.Enqueue(message);
            }
        }
    }



    // Update is called once per frame
    void Update()
    {
        if(msgQueue.Count > 0)
        {
            string message = msgQueue.Dequeue();
            Debug.Log(message);
        }
    }
    public void Close()
    {
        if(socket != null)
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            isConnect = false;
        }
    }
    private void OnDestroy()
    {
        Close();
    }
}
