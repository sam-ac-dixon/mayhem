using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mayhem.Input
{
    // a controller gameobject (eg. a virtual GUI button) should call the
    // 'pressed' function of this class. Other objects can then read the
    // Get/Down/Up state of this button.
    public class VirtualButton
    {
        public string name { get; private set; }
        public bool matchWithInputManager { get; private set; }

        private int m_LastPressedFrame = -5;
        private int m_ReleasedFrame = -5;
        private bool m_Pressed;


        public VirtualButton(string name)
            : this(name, true)
        {
        }


        public VirtualButton(string name, bool matchToInputSettings)
        {
            this.name = name;
            matchWithInputManager = matchToInputSettings;
        }


        // A controller gameobject should call this function when the button is pressed down
        public void Pressed()
        {
            if (m_Pressed)
            {
                return;
            }
            m_Pressed = true;
            m_LastPressedFrame = Time.frameCount;
        }


        // A controller gameobject should call this function when the button is released
        public void Released()
        {
            m_Pressed = false;
            m_ReleasedFrame = Time.frameCount;
        }


        // the controller gameobject should call Remove when the button is destroyed or disabled
        public void Remove()
        {
            InputManager.UnRegisterVirtualButton(name);
        }


        // these are the states of the button which can be read via the cross platform input system
        public bool GetButton
        {
            get { return m_Pressed; }
        }


        public bool GetButtonDown
        {
            get
            {
                return m_LastPressedFrame - Time.frameCount == -1;
            }
        }


        public bool GetButtonUp
        {
            get
            {
                return (m_ReleasedFrame == Time.frameCount - 1);
            }
        }
    }
}
