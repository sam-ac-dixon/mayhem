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
			throw new ArgumentException("Unsupported protocol: " + protocol);
	}

#if UNITY_WEBGL && !UNITY_EDITOR
	[DllImport("__Internal")]
	private static extern int SocketCreate (string url);

	[DllImport("__Internal")]
	private static extern int SocketState (int socketInstance);

	[DllImport("__Internal")]
	private static extern void SocketSend (int socketInstance, string msg);

	[DllImport("__Internal")]
	private static extern int SocketRecv (int socketInstance, byte[] ptr);

	[DllImport("__Internal")]
	private static extern int SocketRecvLength (int socketInstance);

	[DllImport("__Internal")]
	private static extern void SocketClose (int socketInstance);

	[DllImport("__Internal")]
	private static extern int SocketError (int socketInstance, byte[] ptr, int length);

	int m_NativeRef = 0;

	public void Send(string msg)
	{
		SocketSend (m_NativeRef, msg);
	}

	public string Recv()
	{
        int bufsize = SocketRecvLength(m_NativeRef);
		byte[] buffer = new byte[bufsize];
		int result = SocketRecv (m_NativeRef, buffer);

		if (result == 0)
			return null;
		return Encoding.UTF8.GetString (buffer);			
	}

	public IEnumerator Connect()
	{
		m_NativeRef = SocketCreate (mUrl.ToString());

		while (SocketState(m_NativeRef) == 0)
			yield return 0;
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
#else
    WebSocketSharp.WebSocket m_Socket;
	Queue<string> m_Messages = new Queue<string>();
	bool m_IsConnected = false;
	string m_Error = null;

	public IEnumerator Connect()
	{
		m_Socket = new WebSocketSharp.WebSocket(mUrl.ToString());
		m_Socket.OnMessage += (sender, e) => m_Messages.Enqueue (e.Data);
		m_Socket.OnOpen += (sender, e) => m_IsConnected = true;
		m_Socket.OnError += (sender, e) => m_Error = e.Message;
		m_Socket.ConnectAsync();
		while (!m_IsConnected && m_Error == null)
			yield return 0;
	}

	public void Send(string msg)
	{
		m_Socket.Send(msg);
	}

	public string Recv()
	{
		if (m_Messages.Count == 0)
			return null;
		return m_Messages.Dequeue();
	}

	public void Close()
	{
		m_Socket.Close();
	}

	public string error
	{
		get {
			return m_Error;
		}
	}
#endif 
}