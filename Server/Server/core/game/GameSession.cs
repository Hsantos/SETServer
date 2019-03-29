using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.core.game.behaviour;
using Server.core.game.card;

namespace Server.core.game
{
    public class GameSession
    {
        private SessionBehaviour behaviour;
        private List<Card> cardSession;
        private GameServices services;
        public const int TOTAL_CARDS_IN_DEFAULT_ROUND = 12;
        public const int TOTAL_CARDS_IN_EXTRA_ROUND = 3;
        public const int TOTAL_CARDS_TO_MATCH = 3;
        private List<Card> openedList;

        //server session
        public List<Card> defaultCards { get; set; }

        public GameSession(GameServices services)
        {
            this.services = services;
            behaviour = new SessionBehaviour();
            cardSession = behaviour.GenerateShuffleCard().ToList();
            OpenDefaultCards();
        }

        private void OpenDefaultCards()
        {
            openedList = new List<Card>();
            defaultCards = OpenCards(TOTAL_CARDS_IN_DEFAULT_ROUND);
            services.notifyDefaultCards(defaultCards);
            CheckAnyMatch();
        }

        public void OpenCardsAfterMatch(int totalToOpen)
        {
            List<Card> cardsToOpen = OpenCards(totalToOpen);
            if (cardsToOpen != null) services.notifyOpenCardsAfterMatch(cardsToOpen);
            else services.notifyEndSession();
        }

        public void OpenExtraCards()
        {
            List<Card> cardsToOpen = OpenCards(TOTAL_CARDS_IN_EXTRA_ROUND);
            if (cardsToOpen != null) services.notifyExtraCards(cardsToOpen);
            else services.notifyEndSession();
        }

        private List<Card> OpenCards(int total)
        {
            int countCards = total <= cardSession.Count ? total : cardSession.Count;
            if (countCards == 0) return null;

            List<Card> currentOpen = new List<Card>();
            for (int i = 0; i < total; i++)
            {
                openedList.Add(cardSession[i]);
                currentOpen.Add(cardSession[i]);
                cardSession.Remove(cardSession[i]);
            }

            return currentOpen;
        }

        public bool IsMatch(List<Card> matchList)
        {
            return behaviour.IsMatch(matchList);
        }

        public void CheckMatch(List<Card> matchList)
        {
            //        behaviour.IsMatch(matchList);
            //        services.notifyMatchCompleted(null);

            Console.WriteLine("Start Check Match: "  +  matchList.Count  + " | " + IsMatch(matchList));

            if (IsMatch(matchList))
            {
                foreach (var t in matchList)
                {
                    openedList.Remove(t);
                }
                services.notifyMatchCompleted(matchList);
            }
            else services.notifyMatchCompleted(null);
        }

        public void CheckAnyMatch()
        {
            bool anyMatch = false;
            for (int i = 0; i < openedList.Count; i++)
            {
                Card cardOne = openedList[i];

                for (int j = 0; j < openedList.Count; j++)
                {
                    if (i == j) continue;

                    Card cardTwo = openedList[GetIndex(j)];
                    Card cardThree = openedList[GetIndex(j + 1)];

                    if (behaviour.IsMatch(new List<Card>() { cardOne, cardTwo, cardThree }))
                    {
                        Console.WriteLine("POSSIBLE MATCH: " + cardOne + " | " + cardTwo + " | " + cardThree);
                        anyMatch = true;
                    }
                }
            }

           // Debug.LogWarning("ANY MATCH: " + anyMatch);
            if (!anyMatch) OpenExtraCards();
        }

        private int GetIndex(int index)
        {
            if (index >= openedList.Count) return 0;
            return index;
        }

    }
}
