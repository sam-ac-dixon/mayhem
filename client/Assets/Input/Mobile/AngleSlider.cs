using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Mayhem.Input.Mobile
{
    public class AngleSlider : MonoBehaviour
    {
        private Slider m_Slider;
        private VirtualAxis m_AngleQuickChange;
    
        void Start()
        {
            m_Slider = GetComponent<Slider>();
            m_Slider.onValueChanged.AddListener(delegate
            {
                ValueChangeCheck();
            });

            m_AngleQuickChange = new VirtualAxis("AngleQuickChange");


            InputManager.RegisterVirtualAxis(m_AngleQuickChange);
        }

        public void Update()
        {
            m_AngleQuickChange.Update(m_Slider.value);
        }

        public void StopDrag()
        {
            m_Slider.value = 0;

            m_AngleQuickChange.Update(0);
        }

        public void ValueChangeCheck()
        {

        }
    }
}