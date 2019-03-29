using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.core.network.request;

namespace Server.core.network
{
    public interface ClientNetworkServices
    {
        void OnServerUp(ServerNetworkServices server);
        void OnConnectionCallback(ClientNetwork client);
        void OnStartSession();
        void OnReceiveMessage(ClientNetwork clientNetWork, ClientReply reply);
    }
}
