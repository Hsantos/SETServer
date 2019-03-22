using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.core.network.request
{
    [Serializable]
    public class ClientReply
    {
        public RequestAction action;
        public string data;

        public ClientReply(RequestAction action, string data)
        {
            this.action = action;
            this.data = data;
        }
    }
}
