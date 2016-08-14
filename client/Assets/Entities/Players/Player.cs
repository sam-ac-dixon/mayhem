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

#if MOBILE_INPUT
        private float m_PreviousQuickChangeAngle = 0f;

        /// <summary>
        /// Required so that it doesn't gitter due to the slight changes in delta time
        /// </summary>
        private float m_QuickChangeAngle = 0f;

        private float m_QuickChangeRotateSpeedMultiplyer = 5;
#endif

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
#if MOBILE_INPUT

            handle_mobileMovement();
#endif
#if !MOBILE_INPUT
            handle_pcMovement();
#endif

            if (UnityEngine.Input.GetKey(KeyCode.Space))
            {
                int weapon = 4;

                Commands.ToServer.BaseCommand cmd = Commands.ToServer.BaseCommand.CreateCommand(new Commands.ToServer.FireCommand(weapon));

                m_UnsentActions.data.Add(cmd);

                Debug.Log("Shoot");
            }
        }

        private void handle_pcMovement()
        {
            transform.Rotate(new Vector3(0, 0, -Input.InputManager.GetAxis("Horizontal") * m_RotateSpeed * Time.deltaTime));
            transform.position += transform.right * Input.InputManager.GetAxis("Vertical") * m_MovementSpeed * Time.deltaTime;
        }

        float currentAngle = 0f;

        private void handle_mobileMovement()
        {

            if (Input.InputManager.GetAxis("Horizontal") != 0 || Input.InputManager.GetAxis("Vertical") != 0)
            {
                currentAngle = Mathf.Atan2(Input.InputManager.GetAxis("Vertical"), Input.InputManager.GetAxis("Horizontal"));
                currentAngle = currentAngle * Mathf.Rad2Deg;
            }

            m_PreviousQuickChangeAngle = m_QuickChangeAngle;
            float unverifiedQuickChangeAngle = Input.InputManager.GetAxis("AngleQuickChange");

            if (unverifiedQuickChangeAngle != 0 && m_PreviousQuickChangeAngle != unverifiedQuickChangeAngle)
            {
                m_QuickChangeAngle = (unverifiedQuickChangeAngle * (m_RotateSpeed));
            }
            else
            {
                m_QuickChangeAngle = 0;
            }

            if (Input.InputManager.GetAxis("Horizontal") != 0 || Input.InputManager.GetAxis("Vertical") != 0)
            {
                transform.position += transform.right * Time.deltaTime * m_MovementSpeed;
            }

            if (Input.InputManager.GetAxis("Horizontal") != 0 || Input.InputManager.GetAxis("Vertical") != 0 || m_QuickChangeAngle != 0)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, currentAngle + m_QuickChangeAngle));
            }
        }
    }
}