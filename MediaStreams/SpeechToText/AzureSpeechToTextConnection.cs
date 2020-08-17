using Microsoft.CognitiveServices.Speech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaStreams.SpeechToText
{
    public class AzureSpeechToTextConnection
    {
        public string SessionId;
        public SpeechRecognizer SpeechClient;
        public VoiceAudioStream AudioStream;
    }
}