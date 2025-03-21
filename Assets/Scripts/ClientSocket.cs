using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class ClientSocket
{
    private Socket socket;
    private static int CLIENT_BEGIN_ID = 2;
    public int clientID;
    public ClientSocket(Socket clientSocket)
    {
        socket = clientSocket;
        clientID = CLIENT_BEGIN_ID;
        CLIENT_BEGIN_ID++;
    }
    public void SendMessage(string message)
    {
        while(socket!=null)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                socket.Send(data);
            }
            catch
            {
                socket.Close();
            }
        }
    }
    public void ReceiveMessage()
    {
        if (socket == null) return;
        try
        {
            if(socket.Available > 0)
            {
                byte[] result = new byte[1024];
                int resultNum = socket.Receive(result);
                ThreadPool.QueueUserWorkItem(HandleMessage,Encoding.UTF8.GetString(result,0,resultNum));
            }
        }
        catch
        {
            Close();
        }
    }

    private void HandleMessage(object state)
    {
        string message = state as string;
        Console.WriteLine("Client " + clientID + " : " + message);
    }

    public void Close()
    {
        socket.Shutdown(SocketShutdown.Both);
        socket.Close();
    }
}
