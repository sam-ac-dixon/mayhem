using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Mayhem.Entities.Players
{
    public class ConnectedPlayer : Base, IPlayerEntity
    {
        public void ServerUpdate(Commands.PlayerCommandData data)
        {
            if(String.IsNullOrEmpty(m_ID))
            {
                m_ID = data.id;
                this.name = m_ID;
            }

            transform.position = new Vector3(data.x, data.y, 0);
        }

        public virtual void Update()
        {

        }
    }
}
