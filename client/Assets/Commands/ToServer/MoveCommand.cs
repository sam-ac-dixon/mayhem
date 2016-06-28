using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mayhem.Commands.ToServer
{
    [Serializable]
    public class MoveCommand : BaseCommand
    {
        public float acceleration;
        public float rotation;

        public MoveCommand(float acceleration, float rotation)
        {
            this.type = "move";
            this.acceleration = acceleration;
            this.rotation = rotation;
        }
    }
}
