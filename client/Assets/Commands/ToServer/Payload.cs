using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mayhem.Commands.ToServer
{
    [Serializable]
    public class Payload
    {
        public List<BaseCommand> data;

        public Payload()
        {
            data = new List<BaseCommand>();
        }
    }
}