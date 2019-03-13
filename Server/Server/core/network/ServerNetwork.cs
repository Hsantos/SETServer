using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server.core.network
{
    public class ServerNetwork
    {
        System.Threading.Thread SocketThread;
        volatile bool keepReading = false;
        private Socket listener;
        private Socket handler;

        public ServerNetwork()
        {
            StartServer();
        }

        private void StartServer()
        {
            SocketThread = new System.Threading.Thread(NetworkBehaviour);
            SocketThread.IsBackground = true;
            SocketThread.Start();
        }

        private string GetIPAddress()
        {
            IPHostEntry host;
            string localIP = "localhost";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                }

            }
            return localIP;
        }

        private void NetworkBehaviour()
        {
            string data;

            byte[] bytes = new byte[1024];

            Console.WriteLine("IP: " + GetIPAddress());

            IPAddress[] ipArray = Dns.GetHostAddresses(GetIPAddress());
            IPEndPoint localEndPoint = new IPEndPoint(ipArray[0], 1755);

            listener = new Socket(ipArray[0].AddressFamily,SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                while (true)
                {
                    keepReading = true;

                    Console.WriteLine("Waiting for Connection");

                    handler = listener.Accept();
                    Console.WriteLine("Client Connected");    
                    data = null;

                    while (keepReading)
                    {
                        bytes = new byte[1024];
                        int bytesRec = handler.Receive(bytes);
                        Console.WriteLine("Received from Server");

                        if (bytesRec <= 0)
                        {
                            keepReading = false;
                            handler.Disconnect(true);
                            break;
                        }

                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);

                        
                        if (data.IndexOf("<EOF>") > -1) break;
                        
                        System.Threading.Thread.Sleep(1);
                    }

                    Console.WriteLine("Data Received: " +  data);

                    System.Threading.Thread.Sleep(1);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        
        //TODO needs to call
        private void StopServer()
        {
            keepReading = false;

            SocketThread?.Abort();
            
            if (handler != null && handler.Connected)
            {
                handler.Disconnect(false);
                Console.WriteLine("Disconnected!");
            }
        }
    }
}
