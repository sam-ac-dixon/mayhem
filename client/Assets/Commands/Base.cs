using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mayhem.Commands {
    [Serializable]
    public class Base
    {
        public PlayerCommandData player;
        public PlayerCommandData[] otherplayers;
        public BulletCommandData[] bullets;
    }
}