using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace Mayhem.GUI.WeaponSelection
{
    public class WeaponContainer : MonoBehaviour
    {
        private int m_WeaponCount = 5;
        private int m_SelectedWeaponIndex = 1;
        // public float MinimumTransparency = 0.4f;
        // public float MaximumTransparency = 1f;

        private float m_SwipeZoneStartY = Screen.height / 5;
        private float m_SwipeZoneStartX = Screen.width / 2;
        private RectTransform m_SwipeZone;
        public string WeaponChildName = "WeaponIcon";

        void Start()
        {
            m_WeaponCount = transform.childCount - 1; // -1 accounts for the swipe zone;

            m_SwipeZone = transform.FindChild("SwipeZone").GetComponent<RectTransform>();

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

            selectWeapon(((m_WeaponCount - 1) / 2));
        }

        Vector2 startPos;
        float minSwipeDist = 10f;
        bool isSwiping = false;

        void handleMobileTouches()
        {
            foreach (var touch in UnityEngine.Input.touches)
            {

                if(RectTransformUtility.RectangleContainsScreenPoint(m_SwipeZone, touch.position) == false)
                {
                    continue;
                }

                if (touch.phase == TouchPhase.Began)
                {
                    startPos = touch.position;
                    isSwiping = true;  
                }
                else if (touch.phase == TouchPhase.Ended && isSwiping && Mathf.Abs(touch.position.y - startPos.y) > 30) 
                {
                    var swipeDirection = Mathf.Sign(touch.position.y - startPos.y);

                    GetComponent<Text>().text = swipeDirection.ToString();

                    if (swipeDirection > 0)
                    {
                        getNextWeapon();
                    }
                    else if (swipeDirection < 0)
                    {
                        getPreviousWeapon();
                    }

                    isSwiping = false;

                }
            }
        }

        void Update()
        {
#if MOBILE_INPUT
            handleMobileTouches();
#endif

#if !MOBILE_INPUT
            for (int i = (int)KeyCode.Alpha1; i < (int)KeyCode.Alpha1 + m_WeaponCount; i++)
            {
                if (UnityEngine.Input.GetKeyDown((KeyCode)i))
                {
                    selectWeapon(i - (int)KeyCode.Alpha1 + 1); // + 1 Accounts for indexs starting at index of 1
                }
            }
#endif

        }

        private void selectWeapon(int newlySelectedWeapon)
        {
            if (newlySelectedWeapon <= m_WeaponCount && newlySelectedWeapon > 0)
            {

                transform.FindChild(WeaponChildName + m_SelectedWeaponIndex).GetComponent<Image>().color = Color.white;
                m_SelectedWeaponIndex = newlySelectedWeapon;

                transform.FindChild(WeaponChildName + newlySelectedWeapon).GetComponent<Image>().color = Color.red;
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