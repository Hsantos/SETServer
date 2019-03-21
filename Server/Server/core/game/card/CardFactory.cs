using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Server.core.game.card
{
    [Serializable]
    public class CardFactory
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum COLOR
        {
            GREEN,
            RED,
            PURPLE
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public enum AMOUNT
        {
            ONE ,
            TWO ,
            THREE 
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public enum SHADING
        {
            SOLID,
            STRIPED,
            OUTLINED
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public enum SHAPE
        {
            OVAL,
            SQUIGGLE,
            DIAMOND
        }
    }
}
