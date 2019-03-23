
using System;

namespace Server.core.game
{
    [Serializable]
    public class ClientPoint
    {
        public string name;
        public int points;

        public ClientPoint(string name, int points)
        {
            this.name = name;
            this.points = points;
        }
    }
}
