﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mayhem
{
    public class NetworkManager : MonoBehaviour
    {
        public string URL = "ws://192.168.1.61:8001";

        private Queue<Commands.FromServer.Payload> m_UnprocessedCommands;
        private WebSocket m_Socket;
        public float UpdateRate;

        private float m_TimeUntilUpdate;

        public void Awake()
        {
            m_UnprocessedCommands = new Queue<Commands.FromServer.Payload>();
            m_Socket = new WebSocket(new Uri(URL));
            m_TimeUntilUpdate = 0;
        }

        IEnumerator Start()
        {
            yield return StartCoroutine(m_Socket.Connect());

            string[] commands;

            while (m_Socket.IsConnected)
            {
                if (m_TimeUntilUpdate > 0)
                {
                    m_TimeUntilUpdate -= UnityEngine.Time.deltaTime;
                    yield return 0;
                }
                else
                {
                    m_TimeUntilUpdate = UpdateRate;
                }

                commands = m_Socket.Recv();

                if (commands != null)
                {
                    foreach(string cmd in commands)
                    {                       
                        Commands.FromServer.Payload recievedCommand = JsonUtility.FromJson<Commands.FromServer.Payload>(cmd);

                        m_UnprocessedCommands.Enqueue(recievedCommand);
                    }
                }

                if (m_Socket.error != null)
                {
                    Debug.LogError("Error: " + m_Socket.error);
                    break;
                }

                yield return 0;
            }

            m_Socket.Close();
        }

        public void SendData(string commands)
        {
            m_Socket.Send(commands);
        }

        public Commands.FromServer.Payload PopCommand()
        {
            return m_UnprocessedCommands.Dequeue();
        }

        public int GetCommandCount()
        {
            return m_UnprocessedCommands.Count;
        }
    }
}
