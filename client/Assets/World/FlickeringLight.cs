using UnityEngine;
using System.Collections;

namespace Mayhem.Environment
{
    public class FlickeringLight : MonoBehaviour
    {
        private Light m_Light;

        private float[] m_Smoothing = new float[20];

        void Start()
        {
            // Initialize the array.
            for (int i = 0; i < m_Smoothing.Length; i++)
            {
                m_Smoothing[i] = 0.0f;
            }
            m_Light = GetComponent<Light>();
        }

        void Update()
        {
            float sum = 0.0f;

            for (int i = 1; i < m_Smoothing.Length; i++)
            {
                m_Smoothing[i - 1] = m_Smoothing[i];
                sum += m_Smoothing[i - 1];
            }

            // Add the new value at the end of the array.
            m_Smoothing[m_Smoothing.Length - 1] = Random.Range(0.1f, 1.5f);
            sum += m_Smoothing[m_Smoothing.Length - 1];

            // Compute the average of the array and assign it to the
            // light intensity.
            m_Light.intensity = sum / m_Smoothing.Length;
        }
    }
}