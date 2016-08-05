using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Collections.Generic;

namespace Mayhem.Entities.Players
{

    public class Player : ConnectedPlayer
    {
        private float m_MovementSpeed = 3f;
        private float m_RotateSpeed = 100f;
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

        float currentAngle = 0f;
        float quickChangeAngle = 0f;

        float previousQuickChangeAngle = 0f;

        public override void Update()
        {
#if MOBILE_INPUT
            currentAngle = Mathf.Atan2(Input.InputManager.GetAxis("Vertical"), Input.InputManager.GetAxis("Horizontal"));
            currentAngle = currentAngle * Mathf.Rad2Deg;

            previousQuickChangeAngle = quickChangeAngle;
            quickChangeAngle = Input.InputManager.GetAxis("AngleQuickChange");

            if (quickChangeAngle != 0 && previousQuickChangeAngle != quickChangeAngle)
            {
                float newAngle = quickChangeAngle + currentAngle;

                newAngle = newAngle * (m_RotateSpeed * 5) * Time.deltaTime;

                transform.rotation = Quaternion.Euler(new Vector3(0, 0, newAngle));
            }

            if (Input.InputManager.GetAxis("Horizontal") != 0 || Input.InputManager.GetAxis("Vertical") != 0)
            {
                transform.position += transform.right * Time.deltaTime * m_MovementSpeed;
                transform.eulerAngles = new Vector3(0, 0, currentAngle + quickChangeAngle);
            }

#endif
#if !MOBILE_INPUT
            transform.Rotate(new Vector3(0, 0, -Input.InputManager.GetAxis("Horizontal") * m_RotateSpeed * Time.deltaTime));
            transform.position += transform.right * Input.InputManager.GetAxis("Vertical") * m_MovementSpeed * Time.deltaTime;
#endif

            if (UnityEngine.Input.GetKey(KeyCode.Space))
            {
                int weapon = 4;

                Commands.ToServer.BaseCommand cmd = Commands.ToServer.BaseCommand.CreateCommand(new Commands.ToServer.FireCommand(weapon));

                m_UnsentActions.data.Add(cmd);

                Debug.Log("Shoot");
            }
        }
    }
}