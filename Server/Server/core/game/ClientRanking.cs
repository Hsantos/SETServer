using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.core.game
{
    [Serializable]
    public class ClientRanking
    {
        public List<ClientPoint> list;

        public ClientRanking(List<ClientPoint> list)
        {
            this.list = list;
        }
    }
}
