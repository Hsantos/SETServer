using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.core.game.card
{
    [Serializable]
    public class Card
    {
        public CardFactory.COLOR color { get; set; }
        public CardFactory.AMOUNT amount { get; set; }
        public CardFactory.SHADING shading { get; set; }
        public CardFactory.SHAPE shape { get; set; }
        public int id { get; set; }

        public Card(int id, CardFactory.COLOR color, CardFactory.AMOUNT amount, CardFactory.SHADING shading, CardFactory.SHAPE shape)
        {
            this.id = id;
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
            return id == cd.id;
        }
    }
}
