using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.core.game.card;

namespace Server.core.game.client
{
    [Serializable]
    public class ClientDataRequested
    {
        public List<Card> cards;
    }
}
