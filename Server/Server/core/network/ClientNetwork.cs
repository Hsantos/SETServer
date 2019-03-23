using System.Net.Sockets;

namespace Server.core.network
{
    public class ClientNetwork
    {
        public int  id { get; private set; }
        public string name { get; private set; }
        public Socket socket { get; private set; }
        public int points{ get; private set; }


        public ClientNetwork(int id,Socket socket)
        {
            this.id = id;
            this.socket = socket;
            this.points = 0;
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public void UpdatePoints()
        {
            points++;
        }
   }
}
