using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Collections.Generic;

namespace Mayhem.Entities.Players {
    public class Player : ConnectedPlayer
    {
        private float m_MovementSpeed = 3f;
        private float m_RotateSpeed = 5f;
        private Commands.ToServer.Payload m_UnsentActions;

        public Commands.ToServer.Payload Actions
        {
            get
            {
                return m_UnsentActions;
            }
        }

        public float MovementSpeed
        {
            get
            {
                return m_MovementSpeed;
            }
            set
            {
                m_MovementSpeed = value;
            }
        }

        public float RotateSpeed
        {
            get
            {
                return m_RotateSpeed;
            }
            set
            {
                m_RotateSpeed = value;
            }
        }

        public void Awake()
        {
            m_UnsentActions = new Commands.ToServer.Payload();
        }

        public override void Update()
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += transform.right * m_MovementSpeed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(Vector3.forward * m_RotateSpeed);
            }

            if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(Vector3.forward * -m_RotateSpeed);
            }

            if (Input.GetKey(KeyCode.Space))
            {
                int weapon = 4;

                Commands.ToServer.BaseCommand cmd = Commands.ToServer.BaseCommand.CreateCommand(new Commands.ToServer.FireCommand(weapon));

                m_UnsentActions.data.Add(cmd);

                Debug.Log("Shoot");
            }
        }
    }
}