using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Server.core.game.card;
using Server.core.network;
using Newtonsoft.Json;
using Server.core.game.client;

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

        public void OnReceiveMessage(string data)
        {
            if (data == RequestType.DEFAULT_CARDS)
            {
                SendMessage(clients, session.defaultCards);
            }
        }

        private void StartSession()
        {
            session = new GameSession(this);
        } 
        

        public void notifyDefaultCards(List<Card> cards){}

        public void notifyOpenCardsAfterMatch(List<Card> cards)
        {
            throw new NotImplementedException();
        }

        public void notifyExtraCards(List<Card> cards)
        {
            throw new NotImplementedException();
        }

        public void notifyMatchCompleted(List<Card> cards)
        {
            throw new NotImplementedException();
        }

        public void notifyEndSession()
        {
            throw new NotImplementedException();
        }

        private void SendMessage(ClientNetwork client, List<Card> data) => SendMessage(new List<ClientNetwork>() { client }, data);
        private void SendMessage(List<ClientNetwork> clients, List<Card> data)
        {
            ClientDataRequested request = new ClientDataRequested(){cards = data};
            var json = JsonConvert.SerializeObject(request);
            serverInstance.SendMessageToClients(clients, json);
        }

        
    }
}
