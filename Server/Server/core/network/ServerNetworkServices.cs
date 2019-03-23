
using System.Collections.Generic;
using Server.core.network.request;

namespace Server.core.network
{
    public interface ServerNetworkServices
    {
        void SendMessageToClients(List<ClientNetwork> clients, ServerReply reply);
    }
}
