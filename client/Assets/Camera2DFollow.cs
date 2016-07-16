using System;
using UnityEngine;

namespace Mayhem
{
    public class Camera2DFollow : MonoBehaviour
    {
        public Transform target;

        public World.Map m_WorldMap;

        private float rightBound;
        private float leftBound;
        private float topBound;
        private float bottomBound;
        private Vector3 pos;
        private Vector2 spriteBounds;
        private bool m_HasInitialised;

        void Start()
        {
            m_HasInitialised = false;
        }

        // Update is called once per frame
        void Update()
        {
            Camera cam = GetComponent<Camera>();

            float camVertExtent = cam.orthographicSize;
            float camHorzExtent = cam.aspect * camVertExtent;

            float leftBound = m_WorldMap.Bounds.min.x + camHorzExtent;
            float rightBound = m_WorldMap.Bounds.max.x - camHorzExtent;
            float bottomBound = m_WorldMap.Bounds.min.y + camVertExtent;
            float topBound = m_WorldMap.Bounds.max.y - camVertExtent;

            float camX = Mathf.Clamp(target.transform.position.x, leftBound, rightBound);
            float camY = Mathf.Clamp(target.transform.position.y, bottomBound, topBound);

            cam.transform.position = new Vector3(camX, camY, cam.transform.position.z);


        }
    }
}
