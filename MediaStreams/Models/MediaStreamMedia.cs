using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace MediaStreams.Models
{
    public class MediaStreamMedia
    {
        [JsonProperty("track")]
        public string Track { get; set; }

        [JsonProperty("chunk")]
        public int Chunk { get; set; }

        [JsonProperty("timestamp")]
        public int Timestamp { get; set; }

        [JsonProperty("payload")]
        public string Payload { get; set; }
    }
}
