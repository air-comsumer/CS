using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class ServerSocket
{
    private Socket socket;
    private Dictionary<int, ClientSocket> clientDic = new Dictionary<int, ClientSocket>();
    private bool isClose;
    public ServerSocket(string IP, int port,int number)
    {
        isClose = false;
        IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Bind(ipEndPoint);
        try
        {
            socket.Listen(number);
            ThreadPool.QueueUserWorkItem(AcceptClient);
            ThreadPool.QueueUserWorkItem(ReceiveClient);
        }
        catch (SocketException e)
        {
            Debug.Log(e.Message);
        }
    }
    public void Close()
    {
        isClose = true;
        foreach (var client in clientDic.Values)
        {
            client.Close();
        }
        clientDic.Clear();
        socket.Shutdown(SocketShutdown.Both);
        socket.Close();
    }
    private void ReceiveClient(object state)
    {
        while (isClose)
        {
            if(socket.Available > 0)
            {
                foreach(var client in clientDic.Values)
                {
                    client.ReceiveMessage();
                }
            }
        }
    }

    private void AcceptClient(object state)
    {
        while (isClose)
        {
            try
            {
                Socket client = socket.Accept();
                ClientSocket clientSocket = new ClientSocket(client);
                clientDic.Add(clientSocket.clientID, clientSocket);
            }
            catch (SocketException e)
            {
                Debug.Log(e.Message);
            }
        }
    }
}
