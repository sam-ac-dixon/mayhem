using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mayhem
{
    public class GameCore : MonoBehaviour
    {
        /// <summary>
        /// The speed in which the NetworkClient polls for data in ms.
        /// </summary>
        private float m_UpdateRate;

        public GameObject OtherPlayerPrefab;

        public NetworkClient m_NetworkClient;
        private Player m_MainPlayer;

        private int m_FrameRate;

        public int FrameRate
        {
            get
            {
                return m_FrameRate;
            }
            set
            {
                m_UpdateRate = 1000 / value;
                m_FrameRate = value;
                m_NetworkClient.UpdateRate = m_UpdateRate;
            }
        }

        public void Awake()
        {
            m_NetworkClient = GameObject.Find("NetworkManager").GetComponent<NetworkClient>();
            m_MainPlayer = GameObject.Find("MainPlayer").GetComponent<MainPlayer>();

            FrameRate = 30;
        }

        /// <summary>
        /// TO DO:
        ///     The server is only going to send data which affects the client (Is near by it)
        ///     Investigate whether deleting entities which are within the client but not updated by the server is better than
        ///     disabling (not draw or update) if the server hasn't sent their data in a packet and deleting them after n packets/or time.
        ///      
        /// </summary>
        public void Update()
        {
            int currentCommandCount = m_NetworkClient.GetCommandCount();

            for(int i = 0; i < currentCommandCount; i++)
            {
                Commands.Base currentCommand = m_NetworkClient.PopCommand();

                m_MainPlayer.ServerUpdate(currentCommand.player);

                foreach(Commands.PlayerCommandData command in currentCommand.otherplayers)
                {
                    GameObject otherPlayerObj = GameObject.Find(command.id);

                    if(otherPlayerObj == null)
                    { 
                        Vector3 pos = new Vector3(command.x, command.y, command.a);
                        otherPlayerObj = (GameObject)Instantiate(OtherPlayerPrefab, pos, Quaternion.identity);
                    }
             
                    otherPlayerObj.GetComponent<Player>().ServerUpdate(command);
                }
            }
        }
    }
}
