using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mayhem.Commands.ToServer
{
    [Serializable]
    public class FireCommand : BaseCommand
    {
        public int weapon;

        public FireCommand(int weapon)
        {
            this.type = "fire";
            this.weapon = weapon;
        }
    }
}
