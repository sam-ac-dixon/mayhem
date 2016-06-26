using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using UnityEngine;
using System.Runtime.InteropServices;

public class WebSocket
{
    private Uri mUrl;

    public WebSocket(Uri url)
    {
        mUrl = url;

        string protocol = mUrl.Scheme;
        if (!protocol.Equals("ws") && !protocol.Equals("wss"))
        {
            throw new ArgumentException("Unsupported protocol: " + protocol);
        }
    }

#if UNITY_WEBGL && !UNITY_EDITOR
	[DllImport("__Internal")]
	private static extern int SocketCreate (string url);

	[DllImport("__Internal")]
	private static extern int SocketState (int socketInstance);

	[DllImport("__Internal")]
	private static extern void SocketSend (int socketInstance, byte[] ptr, int length);

	[DllImport("__Internal")]
	private static extern void SocketRecv (int socketInstance, byte[] ptr);

	[DllImport("__Internal")]
	private static extern int AllMessageLength (int socketInstance);

    [DllImport("__Internal")]
	private static extern int NextMessageLength (int socketInstance);

	[DllImport("__Internal")]
	private static extern void SocketClose (int socketInstance);

	[DllImport("__Internal")]
	private static extern int SocketError (int socketInstance, byte[] ptr, int length);

	int m_NativeRef = 0;

	public void Send(string message)
	{
        byte[] buffer = Encoding.UTF8.GetBytes(message);
		SocketSend (m_NativeRef, buffer, buffer.Length);
	}

	public string[] Recv()
	{
		int totalLength = AllMessageLength(m_NativeRef);
        int currentLength = 0;
        List<string> currentMessages = new List<string>();

		if (totalLength == 0)
        {
			return null;
        }

        while(currentLength != totalLength)
        {
            Application.ExternalCall("console.log", "Current Length: " +  currentLength.ToString());

            Application.ExternalCall("console.log", "Total Length: " + totalLength.ToString());
            int msgLength = NextMessageLength(m_NativeRef);
		    byte[] buffer = new byte[msgLength];

		    SocketRecv(m_NativeRef, buffer);

            currentLength += msgLength;

            currentMessages.Add(Encoding.UTF8.GetString(buffer));
        }
        Application.ExternalCall("console.log", currentMessages.Count);
		return currentMessages.ToArray();
	}

	public IEnumerator Connect()
	{
		m_NativeRef = SocketCreate (mUrl.ToString());

		while (SocketState(m_NativeRef) == 0)
        {  
			yield return 0;
        }
	}
 
	public void Close()
	{
		SocketClose(m_NativeRef);
	}

	public string error
	{
		get {
			const int bufsize = 1024;
			byte[] buffer = new byte[bufsize];
			int result = SocketError (m_NativeRef, buffer, bufsize);

			if (result == 0)
				return null;

			return Encoding.UTF8.GetString (buffer);				
		}
	}

    public bool IsConnected
    {
        get
        {
            return SocketState(m_NativeRef) == 1;
        }
    }
#else
    WebSocketSharp.WebSocket m_Socket;
    Queue<string> m_Messages = new Queue<string>();
    bool m_IsConnected = false;
    string m_Error = null;

    public bool IsConnected
    {
        get
        {
            return m_IsConnected;
        }
    }

    public IEnumerator Connect()
    {
        m_Socket = new WebSocketSharp.WebSocket(mUrl.ToString());
        m_Socket.OnMessage += (sender, e) => m_Messages.Enqueue(e.Data);
        m_Socket.OnOpen += (sender, e) => m_IsConnected = true;
        m_Socket.OnError += (sender, e) => m_Error = e.Message;
        m_Socket.ConnectAsync();

        while (!m_IsConnected && m_Error == null)
        {
            yield return 0;
        }
    }

    public void Close()
    {
        m_Socket.Close();
    }

    public string error
    {
        get
        {
            return m_Error;
        }
    }

    public void Send(string str)
    {
        m_Socket.Send(Encoding.UTF8.GetBytes(str));
    }

    public string[] Recv()
    {
        string[] messages = new string[m_Messages.Count];

        for (int i = 0; i < messages.Length; i++)
        {
            messages[i] = m_Messages.Dequeue();
        }

        return messages;
    }
#endif 
}