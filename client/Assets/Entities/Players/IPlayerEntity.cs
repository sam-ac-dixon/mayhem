using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mayhem.Entities.Players
{
    public interface IPlayerEntity
    {
        void ServerUpdate(Commands.PlayerCommandData data);
    }
}
