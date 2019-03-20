using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.core.game.card;

namespace Server.core.game
{
    public interface GameServices
    {
        void notifyDefaultCards(List<Card> cards);
        void notifyOpenCardsAfterMatch(List<Card> cards);
        void notifyExtraCards(List<Card> cards);
        void notifyMatchCompleted(List<Card> cards);
        void notifyEndSession();
    }
}
