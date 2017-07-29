using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BitfinexClientSharp.Dtos
{
    public class Request
    {
        [JsonProperty("event")]
        [JsonConverter(typeof(StringEnumConverter))]
        public EventType Event { get; set; }
        [JsonProperty("channel")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ChannelType Channel { get; set; }
        [JsonProperty("pair")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Pair Pair { get; set; }
    }
}