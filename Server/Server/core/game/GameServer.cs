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

            if (clients.Count == 1)
            {
                StartSession();
            }
        }

        public void OnSendMessage(string data)
        {
            throw new NotImplementedException();
        }

        public void OnReceiveMessage(ClientReply reply)
        {
            switch (reply.action)
            {
                case RequestAction.DEFAULT_CARDS:
                    SendMessage(reply.action,clients, session.defaultCards);
                break;
                case RequestAction.MATCH:
                    List<Card> cards = JsonConvert.DeserializeObject<List<Card>>(reply.data);
                    session.CheckMatch(cards);
                break;
                    
                default:
                    throw  new Exception("UNKNOWN REPLY: "  + reply);
            }
        }

        private void StartSession()
        {
            session = new GameSession(this);
        } 
        
        public void notifyDefaultCards(List<Card> cards){}

        public void notifyOpenCardsAfterMatch(List<Card> cards)    
        {
            //SendMessage(RequestAction.CARDS_AFTER_MATCH, clients, cards);
        }

        public void notifyExtraCards(List<Card> cards)
        {
            throw new NotImplementedException();
        }

        public void notifyMatchCompleted(List<Card> cards)
        {
            SendMessage(RequestAction.MATCH, clients, cards);
        }

        public void notifyEndSession()
        {
            throw new NotImplementedException();
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
