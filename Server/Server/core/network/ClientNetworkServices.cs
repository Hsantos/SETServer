using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.core.network
{
    public interface ClientNetworkServices
    {
        void OnServerUp(ServerNetworkServices server);
        void OnConnectionCallback(ClientNetwork client);
        void OnSendMessage(string data);
        void OnReceiveMessage(string data);
    }
}
