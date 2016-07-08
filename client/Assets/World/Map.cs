using UnityEngine;
using System.Collections;

namespace Mayhem.World
{
    public class Map : MonoBehaviour
    {
        public Vector2 Size;
        public GameObject MapTilePrefab;

        // Use this for initialization
        void Start()
        {
            float tileWidth = MapTilePrefab.GetComponent<RectTransform>().rect.width;
            float tileHeight = MapTilePrefab.GetComponent<RectTransform>().rect.height;

            for (float x = 0; x < Size.x; x += tileWidth)
            {
                for (float y = 0; y < Size.y; y += tileHeight)
                {
                    var obj = (GameObject)Instantiate(MapTilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                    obj.name = "MapTile(" + x.ToString() + ", " + y.ToString() + ")";
                    
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
