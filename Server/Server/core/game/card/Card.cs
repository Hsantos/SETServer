using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.core.game.card
{
    [System.Serializable]
    public class Card
    {
        public CardFactory.COLOR color { get; private set; }
        public CardFactory.AMOUNT amount { get; private set; }
        public CardFactory.SHADING shading { get; private set; }
        public CardFactory.SHAPE shape { get; private set; }

        public Card(CardFactory.COLOR color, CardFactory.AMOUNT amount, CardFactory.SHADING shading, CardFactory.SHAPE shape)
        {
            this.color = color;
            this.amount = amount;
            this.shading = shading;
            this.shape = shape;
        }

        public override string ToString()
        {
            return "[" + color + "," + amount + "," + shading + "," + shape + "]";
        }

        public override bool Equals(object obj)
        {
            Card cd = obj as Card;
            return color == cd.color && amount == cd.amount && shading == cd.shading && shape == cd.shape;
        }
    }
}
