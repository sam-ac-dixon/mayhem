using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mayhem.Input
{
    // virtual axis and button classes - applies to mobile input
    // Can be mapped to touch joysticks, tilt, gyro, etc, depending on desired implementation.
    // Could also be implemented by other input devices - kinect, electronic sensors, etc
    public class VirtualAxis
    {
        public string name { get; private set; }
        private float m_Value;
        public bool matchWithInputManager { get; private set; }


        public VirtualAxis(string name)
            : this(name, true)
        {
        }


        public VirtualAxis(string name, bool matchToInputSettings)
        {
            this.name = name;
            matchWithInputManager = matchToInputSettings;
        }


        // removes an axes from the cross platform input system
        public void Remove()
        {
            InputManager.UnRegisterVirtualAxis(name);
        }


        // a controller gameobject (eg. a virtual thumbstick) should update this class
        public void Update(float value)
        {
            m_Value = value;
        }


        public float GetValue
        {
            get { return m_Value; }
        }


        public float GetValueRaw
        {
            get { return m_Value; }
        }
    }
}
