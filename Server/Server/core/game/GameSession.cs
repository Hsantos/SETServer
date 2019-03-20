﻿using System;
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
        public int userPoints { get; private set; }
        public int timeSession { get; private set; }
        private List<Card> openedList;

        //server session
        public List<Card> defaultCards { get; set; }


        public GameSession(GameServices services)
        {
            this.services = services;
            behaviour = new SessionBehaviour();
            cardSession = behaviour.GenerateShuffleCard().ToList();
            OpenDefaultCards();
            userPoints = 0;
            timeSession = 60;
        }

        private void OpenDefaultCards()
        {
            openedList = new List<Card>();
            defaultCards = OpenCards(TOTAL_CARDS_IN_DEFAULT_ROUND);
            services.notifyDefaultCards(defaultCards);
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

        public void CheckMatch(List<Card> matchList)
        {
            //        behaviour.IsMatch(matchList);
            //        services.notifyMatchCompleted(null);

            if (behaviour.IsMatch(matchList))
            {
                for (int i = 0; i < matchList.Count; i++)
                {
                    openedList.Remove(matchList[i]);
                }
                services.notifyMatchCompleted(matchList);
                OpenCardsAfterMatch(matchList.Count);
                userPoints++;
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
                        //Debug.LogWarning("POSSIBLE MATCH: " + cardOne + " | " + cardTwo + " | " + cardThree);
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

        public int UserTime()
        {
            if (timeSession > 0) timeSession--;
            else services.notifyEndSession();
            return timeSession;
        }
    }
}
