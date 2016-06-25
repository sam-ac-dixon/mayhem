using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mayhem
{
    public class NetworkClient : MonoBehaviour
    {
        public string URL = "ws://192.168.1.61:8001";

        private Queue<Commands.Base> m_UnprocessedCommands;
        private WebSocket m_Socket;
        public float UpdateRate;

        private float m_TimeUntilUpdate;

        public void Awake()
        {
            m_UnprocessedCommands = new Queue<Commands.Base>();
            m_Socket = new WebSocket(new Uri(URL));
            m_TimeUntilUpdate = 0;
        }

        IEnumerator Start()
        {
            yield return StartCoroutine(m_Socket.Connect());

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

                string command = m_Socket.RecvString();

                if (command != null)
                {
                    Commands.Base recievedCommand = JsonUtility.FromJson<Commands.Base>(command);

                    m_UnprocessedCommands.Enqueue(recievedCommand);
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

        public Commands.Base PopCommand()
        {
            return m_UnprocessedCommands.Dequeue();
        }

        public int GetCommandCount()
        {
            return m_UnprocessedCommands.Count;
        }
    }
}
