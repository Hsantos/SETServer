using System;
using Server.core.game;
using Server.core.network;

namespace Server
{
    public class Program
    {
        private static ServerNetwork network;
        private static GameServer gameServer;
        
        static void Main(string[] args)
        {
            gameServer = new GameServer();
            network = new ServerNetwork(gameServer);

            Console.ReadLine();

        }
    }
}
