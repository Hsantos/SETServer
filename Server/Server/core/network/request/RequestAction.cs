using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Server.core.network.request
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum RequestAction
    {
        START_SESSION,
        DEFAULT_CARDS,
        CARDS_AFTER_MATCH,
        MATCH,
        EXTRA_CARDS,
        END_SESSION
    }
}
