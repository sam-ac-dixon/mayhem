using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mayhem.Entities
{
    public class Base  : MonoBehaviour
    {
        protected string m_ID;

        public string MyID
        {
            get
            {
                return m_ID;
            }
        }
    }
}
