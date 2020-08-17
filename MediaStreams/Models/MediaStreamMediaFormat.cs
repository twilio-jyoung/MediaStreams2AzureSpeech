using Newtonsoft.Json;

namespace MediaStreams.Models
{
    public class MediaStreamMediaFormat
    {
        [JsonProperty("encoding")]
        public string Encoding { get; set; }

        [JsonProperty("sampleRate")]
        public string SampleRate { get; set; }

        [JsonProperty("channels")]
        public int Channels { get; set; }

        public override string ToString()
        {
            return $"Encoding={Encoding} SampleRate={SampleRate} Channels={Channels}";
        }
    }
}
