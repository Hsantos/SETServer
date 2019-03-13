using System;
using Server.core.network;

namespace Server
{
    public class Program
    {
        private static ServerNetwork server;

        static void Main(string[] args)
        {
            server = new ServerNetwork();
            Console.ReadLine();

        }
    }
}
