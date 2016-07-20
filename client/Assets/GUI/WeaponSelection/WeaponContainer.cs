using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace Mayhem.GUI.WeaponSelection
{
    public class WeaponContainer : MonoBehaviour
    {
        public GameObject WeaponIconPrefab;
        public int WeaponCount = 5;
        private GameObject[] weapons;
        private int m_SelectedWeaponIndex;
        // public float MinimumTransparency = 0.4f;
        // public float MaximumTransparency = 1f;

        void Start()
        {
            weapons = new GameObject[WeaponCount];

            Vector3 pos = new Vector3(
                transform.position.x - (WeaponIconPrefab.GetComponent<RectTransform>().rect.width / 2),  // Offset so that the entire icon's width is on screen. This is used as the origin is the center of the texture
                transform.position.y - (WeaponIconPrefab.GetComponent<RectTransform>().rect.height * ((WeaponCount - 1) / 2)), // Find the top most y coordinate which fits all icons on the screen
                transform.position.z);
            // float alpha = 1f;
            float middleIndex = (float)Math.Round((float)WeaponCount / 2f, MidpointRounding.AwayFromZero);

            for (int i = 0; i < WeaponCount; i++)
            {
                weapons[i] = (GameObject)Instantiate(WeaponIconPrefab, pos, Quaternion.identity);
                weapons[i].transform.SetParent(transform);
                weapons[i].transform.localPosition = pos;

                pos.y -= WeaponIconPrefab.GetComponent<RectTransform>().rect.height;
            }

            //// MaximumTransparency = MinimumTransparency provides the amount of transparency we have to play with
            //// ((WeaponCount - 1) / 2) returns the amount of icons either side of the middle icon.
            //// This middle icon is always set to the maximum transparency
            //float alphaChange = (MaximumTransparency - MinimumTransparency) / ((WeaponCount - 1) / 2);
            //float currentAlpha = 0f;

            //for (int i = 0; i < middleIndex; i++)
            //{
            //    currentAlpha += alphaChange;
            //    weapons[i].GetComponent<Image>().color = new Color(1, 1, 1, currentAlpha);
            //}

            //weapons[(int)middleIndex].GetComponent<Image>().color = new Color(1, 1, 1, MaximumTransparency);

            //currentAlpha = 1f;

            //for (int i = (int)middleIndex; i < WeaponCount; i++)
            //{
            //    currentAlpha -= alphaChange;
            //    weapons[i].GetComponent<Image>().color = new Color(1, 1, 1, currentAlpha);
            //}

            selectWeapon(((WeaponCount - 1) / 2));
        }

        float startTime;
        Vector2 startPos;
        float minSwipeDist = 10f;

        void handleMobileTouches()
        {
            if (UnityEngine.Input.touchCount > 0)
            {
                var touch = UnityEngine.Input.touches[0];

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        // Only begin if its within the weapon container area
                        startPos = touch.position;
                        startTime = Time.time;
                        break;

                    case TouchPhase.Ended:

                        var swipeTime = Time.time - startTime;
                        // ensure only veritcle swipes affect it
                        var swipeDist = (touch.position - startPos).magnitude;

                        if (swipeDist > minSwipeDist)
                        {
                            var swipeDirection = Mathf.Sign(touch.position.y - startPos.y);

                            Debug.Log(swipeDirection);
                            if (swipeDirection > 0)
                            {
                                getNextWeapon();
                            }
                            else
                            {
                                getPreviousWeapon();
                            }
                        }

                        break;
                }
            }
        }

        void Update()
        {
#if MOBILE_INPUT
            handleMobileTouches();
#endif

#if !MOBILE_INPUT
            for(int i = (int)KeyCode.Alpha1; i < (int)KeyCode.Alpha1 + WeaponCount; i++)
            {
                if (UnityEngine.Input.GetKeyDown((KeyCode)i))
                {
                    selectWeapon(i - (int)KeyCode.Alpha1);
                }
            }
#endif

        }

        private void selectWeapon(int newlySelectedWeapon)
        {
            if(newlySelectedWeapon < WeaponCount && newlySelectedWeapon >= 0)
            {
                weapons[m_SelectedWeaponIndex].GetComponent<Image>().color = Color.white;
                m_SelectedWeaponIndex = newlySelectedWeapon;

                weapons[m_SelectedWeaponIndex].GetComponent<Image>().color = Color.red;
            }  
        }

        private void getNextWeapon()
        {
            selectWeapon(m_SelectedWeaponIndex + 1);
        }

        private void getPreviousWeapon()
        {
            selectWeapon(m_SelectedWeaponIndex - 1);
        }
    }
}