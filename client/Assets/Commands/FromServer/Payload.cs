using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mayhem.Commands.FromServer {
    [Serializable]
    public class Payload
    {
        public PlayerCommandData player;
        public PlayerCommandData[] otherplayers;
        public BulletCommandData[] bullets;
    }
}