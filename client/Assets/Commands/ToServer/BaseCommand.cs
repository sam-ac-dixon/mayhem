using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mayhem.Commands.ToServer
{
    [Serializable]
    public class BaseCommand
    {
        public int id;
        public string type;
        public long timestamp;

        private static int m_NextID = 0;

        public static BaseCommand CreateCommand(BaseCommand command)
        {
            command.id = m_NextID++;
            command.timestamp = DateTime.UtcNow.Ticks;

            return command;
        }
    }
}
