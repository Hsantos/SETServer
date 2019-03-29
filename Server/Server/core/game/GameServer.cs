using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Server.core.game.card;
using Server.core.network;
using Newtonsoft.Json;
using Server.core.game.client;
using Server.core.network.request;

namespace Server.core.game
{
    public class GameServer: ClientNetworkServices, GameServices
    {
        private ServerNetworkServices serverInstance;
        private List<ClientNetwork> clients;
        private GameSession session;
        public const int TOTAL_CLIENTS_TO_START = 1;
        public GameServer()
        {
            clients = new List<ClientNetwork>();
        }

     
        public void OnServerUp(ServerNetworkServices server)
        {
            serverInstance = server;
        }

        public void OnConnectionCallback(ClientNetwork data)
        {
            clients.Add(data);
        }

        public void OnStartSession()
        {
            StartSession();
        }

        public void OnSendMessage(string data)
        {
            throw new NotImplementedException();
        }

        public void OnReceiveMessage(ClientNetwork clientNetwork, ClientReply reply)
        {
            switch (reply.action)
            {
                case RequestAction.DEFAULT_CARDS:
                    SendMessage(reply.action,clients, session.defaultCards);
                break;
                case RequestAction.MATCH:

                    List<Card> cards = JsonConvert.DeserializeObject<List<Card>>(reply.data);

                    if (session.IsMatch(cards)) UpdatePoints(clientNetwork);
                   
                    session.CheckMatch(cards);

                break;

                case RequestAction.CARDS_AFTER_MATCH:

                    session.OpenCardsAfterMatch(3);

                    break;

                default:
                    throw  new Exception("UNKNOWN REPLY: "  + reply);
            }
        }

        private void UpdatePoints(ClientNetwork clientNetwork)
        {
            foreach (var t in clients)
            {
                if (t.id == clientNetwork.id)
                {
                    clientNetwork.UpdatePoints();
                }
            }
        }

        private void StartSession()
        {
            Console.WriteLine("Init Session");
            session = new GameSession(this);
        } 
        
        public void notifyDefaultCards(List<Card> cards){}

        public void notifyOpenCardsAfterMatch(List<Card> cards)=> SendMessage(RequestAction.CARDS_AFTER_MATCH, clients, cards);

        public void notifyExtraCards(List<Card> cards)
        {
//            SendMessage(RequestAction.EXTRA_CARDS, clients, cards);
        } 
        
        public void notifyMatchCompleted(List<Card> cards)=> SendMessage(RequestAction.MATCH, clients, cards);
        
        public void notifyEndSession()
        {
            List<ClientPoint> points  = new List<ClientPoint>();

            foreach (var t in clients)
                points.Add(new ClientPoint(t.name, t.points));

            ClientRanking ranking = new ClientRanking(points);

            ServerReply reply = new ServerReply(RequestAction.END_SESSION, JsonConvert.SerializeObject(ranking));
            serverInstance.SendMessageToClients(clients, reply);
        }

        private void SendMessage(RequestAction action,ClientNetwork client, List<Card> data) => SendMessage(action,new List<ClientNetwork>() { client }, data);
        private void SendMessage(RequestAction action, List<ClientNetwork> clients, List<Card> data)
        {
            ServerReply reply = new ServerReply(action, JsonConvert.SerializeObject(data));
            serverInstance.SendMessageToClients(clients, reply);

            //            ClientDataRequested request = new ClientDataRequested(){cards = data};
            //            var json = JsonConvert.SerializeObject(request);
            //            serverInstance.SendMessageToClients(clients, json);
        }

        
    }
}
