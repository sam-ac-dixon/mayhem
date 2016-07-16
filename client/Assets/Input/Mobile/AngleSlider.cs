using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Mayhem.Input.Mobile
{
    public class AngleSlider : MonoBehaviour
    {
        private Slider m_Slider;
        private VirtualAxis m_AngleQuickChange;
        private bool m_EnableAxisUpdate;
        private float m_AmountChanged;

        public float DeltaChange = 20f;

        // Use this for initialization
        void Start()
        {
            m_Slider = GetComponent<Slider>();
            m_Slider.onValueChanged.AddListener(delegate {
                ValueChangeCheck();
            });

            m_AngleQuickChange = new VirtualAxis("AngleQuickChange");


            InputManager.RegisterVirtualAxis(m_AngleQuickChange);
            m_EnableAxisUpdate = false;
            m_AmountChanged = 0;

        }

        public void Update()
        {
           

            if (m_AmountChanged + m_Slider.value > m_AmountChanged && m_AmountChanged <= DeltaChange)
            {
                m_AmountChanged += m_Slider.value;
                m_EnableAxisUpdate = true;
            }
            else if (m_AmountChanged + m_Slider.value < m_AmountChanged && m_AmountChanged >= -DeltaChange)
            {
                m_AmountChanged += m_Slider.value;
                m_EnableAxisUpdate = true;
            }
            else
            {
                m_EnableAxisUpdate = false;
            }

            if (m_AmountChanged > DeltaChange || m_AmountChanged < -DeltaChange)
            {
                m_EnableAxisUpdate = false;
                m_AngleQuickChange.Update(0);

            }
            else
            {
                m_EnableAxisUpdate = true;
                m_AngleQuickChange.Update(m_Slider.value);

            }

            Debug.Log(m_AmountChanged);
        }

        public void BeginDrag() {
            m_EnableAxisUpdate = true;
        }

        public void StopDrag()
        {
            m_Slider.value = 0;
            m_EnableAxisUpdate = false;
            m_AmountChanged = 0;
            m_AngleQuickChange.Update(0);

        }

        public void ValueChangeCheck()
        {
           
        }
    }
}