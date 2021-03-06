﻿using UnityEngine;
using System.Collections;
using System;
using System.Text;

public class Player : MonoBehaviour {
    private float m_MovementSpeed = 3f;
    private float m_RotateSpeed = 5f;

    private string m_ID;

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

    // Use this for initialization
    IEnumerator Start()
    {
        WebSocket w = new WebSocket(new Uri("ws://192.168.1.61:8001"));
        yield return StartCoroutine(w.Connect());

        while (true)
        {
            string command = w.Recv();

            if (command != null)
            {
                Command recvCommand = JsonUtility.FromJson<Command>(command);

                if (m_ID == null)
                {
                    m_ID = recvCommand.id;
                    w.Send("Recieved and set my ID to: " + m_ID);
                }
            }
            if (w.error != null)
            {
                Debug.LogError("Error: " + w.error);
                break;
            }
            yield return 0;
        }

        w.Close();
    }

    // Update is called once per frame
    void FixedUpdate() {
	    if(Input.GetKey(KeyCode.W))
        {
            transform.position += transform.right * m_MovementSpeed * Time.deltaTime;
        }

        if(Input.GetKey(KeyCode.A))
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
