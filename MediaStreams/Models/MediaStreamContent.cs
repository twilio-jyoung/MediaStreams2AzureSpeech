using Newtonsoft.Json;

namespace MediaStreams.Models
{
    public class MediaStreamContent
    {
        [JsonProperty("event")]
        public string Event { get; set; }

        [JsonProperty("sequenceNumber")]
        public string SequenceNumber { get; set; }

        [JsonProperty("media")]
        public MediaStreamMedia Media { get; set; }

        [JsonProperty("streamSid")]
        public string StreamSid { get; set; }

        [JsonProperty("protocol")]
        public string Protocol { get; set; }

        [JsonProperty("start")]
        public MediaStreamStart Start { get; set; }

        [JsonProperty("stop")]
        public MediaStreamStop Stop { get; set; }
    }
}
