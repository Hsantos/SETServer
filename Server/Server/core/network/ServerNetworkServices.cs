using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.core.network
{
    public interface ServerNetworkServices
    {

        void SendMessageToClients(List<ClientNetwork> clients, string data);
        
    }
}
