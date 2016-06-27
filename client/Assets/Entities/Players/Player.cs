using UnityEngine;
using System.Collections;
using System;
using System.Text;

namespace Mayhem.Entities.Players {
    public class Player : ConnectedPlayer
    {
        private float m_MovementSpeed = 3f;
        private float m_RotateSpeed = 5f;

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
                Debug.Log("Shoot");
            }
        }
    }
}