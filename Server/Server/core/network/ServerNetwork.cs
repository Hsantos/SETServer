using System;
using System.Collections.Generic;
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
//        private Socket handler;

        private List<ClientNetwork> clientsHandler;
        private int countId;
        public ServerNetwork()
        {
            clientsHandler = new List<ClientNetwork>();
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

                    var handler = listener.Accept();
                    Console.WriteLine("Client Connected");    
                    data = null;

                    ClientNetwork cl = new ClientNetwork(GenerateId(), handler);
                    clientsHandler.Add(cl);

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

                    //SEND MESSAGE TO CLIENT
                    SendMessageToClient(handler,"client: " + cl.id + " connection complete");
                    CheckClientIds();

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
            
            if (clientsHandler != null && clientsHandler.Count>0)
            {
                foreach (var t in clientsHandler)
                {
                    if(t.socket.Connected) t.socket.Disconnect(false);
                }

                Console.WriteLine("Disconnected All !");
            }
        }


        private void CheckClientIds()
        {
            string clients = "";
            foreach (var t in clientsHandler)
            {
                clients += t.id +  " , ";
            }
            Console.WriteLine("Active clients: " +  clients);
        }


        private int GenerateId()
        {
            countId++;
            return countId;
        }

        private void SendMessageToClient(Socket handler, string message)
        {
            message += "<EOF>";
            byte[] byteData = Encoding.ASCII.GetBytes(message);

            foreach (var t in clientsHandler)
            {
                if (t.socket == handler)
                {
                    // Begin sending the data to the remote device.  
                    handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
                }
            }
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

    }
}
