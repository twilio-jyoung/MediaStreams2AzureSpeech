using Newtonsoft.Json;

namespace MediaStreams.Models
{
    public class MediaStreamStart
    {
        [JsonProperty("accountSid")]
        public string AccountSid { get; set; }

        [JsonProperty("streamSid")]
        public string StreamSid { get; set; }

        [JsonProperty("callSid")]
        public string CallSid { get; set; }

        [JsonProperty("tracks")]
        public string[] Tracks { get; set; }

        [JsonProperty("mediaFormat")]
        public MediaStreamMediaFormat MediaFormat { get; set; }
    }
}
