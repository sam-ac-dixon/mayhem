using UnityEngine;
using System.Collections;

namespace Mayhem.World
{
    public class Map : MonoBehaviour
    {
        public Vector2 Size;
        public GameObject MapTilePrefab;
        public Rect Bounds
        {
            get
            {
                return m_Bounds;
            }
        }

        private Rect m_Bounds;

        // Use this for initialization
        void Start()
        {
            RectTransform tileRect = MapTilePrefab.GetComponent<RectTransform>();
            float tileWidth = tileRect.rect.width;
            float tileHeight = tileRect.rect.height;

            float tileXCount = Size.x / tileWidth;
            float tileYCount = Size.y / tileHeight;

            m_Bounds = new Rect();

            m_Bounds.min = new Vector2(-(tileWidth / 2.0f), -(tileHeight / 2.0f));
            m_Bounds.max = new Vector2(Size.x, Size.y - (tileHeight / 2.0f));

            for (float x = 0; x < tileXCount; x++)
            {
                for (float y = 0; y < tileYCount; y++)
                {
                    Vector3 pos = new Vector3(x * tileWidth, y * tileHeight, 0);

                    var obj = (GameObject)Instantiate(MapTilePrefab, pos, Quaternion.identity);
                    obj.name = "MapTile(" + pos.ToString() + ")";
                    
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
