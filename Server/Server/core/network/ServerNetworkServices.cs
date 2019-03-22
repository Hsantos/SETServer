using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.core.network.request;

namespace Server.core.network
{
    public interface ServerNetworkServices
    {
        void SendMessageToClients(List<ClientNetwork> clients, ServerReply reply);
    }
}
