using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mayhem.Commands.FromServer
{
    [Serializable]
    public class PlayerCommandData
    {
        public string id;
        public float x;
        public float y;
        public float a;
    }
}