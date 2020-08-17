using Newtonsoft.Json;

namespace MediaStreams.Models
{
    public class MediaStreamStop
    {
        [JsonProperty("accountSid")]
        public string AccountSid { get; set; }

        [JsonProperty("callSid")]
        public string CallSid { get; set; }
    }
}