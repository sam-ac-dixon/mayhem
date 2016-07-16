using UnityEngine;
using System.Collections;

namespace Mayhem
{
    public class CameraBounder : MonoBehaviour
    {
        private Vector2 m_WorldSize;
        private Transform m_Player;
        private Camera m_MyCamera;

        void Start()
        {
            m_WorldSize = GameObject.Find("World").GetComponent<Mayhem.World.Map>().Size;
            m_Player = GameObject.FindWithTag("Player").transform;
            m_MyCamera = GetComponent<Camera>();
        }

        // Update is called once per frame
        void Update()
        {
            Vector2 pos = Vector2.zero;

            //pos.x = Mathf.Clamp(pos.x, leftBound, rightBound);
            //pos.y = Mathf.Clamp(pos.y, bottomBound, topBound);
            m_MyCamera.transform.position = pos;
        }
    }
}
