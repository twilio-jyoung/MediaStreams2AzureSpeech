using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaStreams.SpeechToText
{
    public class AzureSpeechRecognizer
    {
        private static SpeechConfig _speechConfig = SpeechConfig.FromSubscription("", "eastus");
        private static TaskCompletionSource<int> _stopRecognition = new TaskCompletionSource<int>();
        private static Dictionary<string, AzureSpeechToTextConnection> _connections = new Dictionary<string, AzureSpeechToTextConnection>();

        // audio started coming in, setup the necessary sessions with Azure's Speech Recognition Engine and add it to the list of connections made.
        public static async Task AudioStart(string streamId)
        {

            // We will continuously write incoming audio to this stream.


            // setup Azure to recieve our stream - https://www.twilio.com/docs/voice/twiml/stream#message-start

            /*
                        var audioStream = new VoiceAudioStream();
                        var audioFormat = AudioStreamFormat.GetWaveFormatPCM(8000, 8, 1);
                        var audioConfig = AudioConfig.FromStreamInput(audioStream, audioFormat);
                        var recognizerClient = new SpeechRecognizer(_speechConfig, audioConfig);
            */

            /*          
                        var pushStream = AudioInputStream.CreatePushStream(AudioStreamFormat.GetCompressedFormat(AudioStreamContainerFormat.MULAW));
                        var audioInput = AudioConfig.FromStreamInput(pushStream);
                        var recognizerClient = new SpeechRecognizer(_speechConfig, audioInput);
            */

            var audioStream = new VoiceAudioStream();
            var audioConfig = AudioConfig.FromStreamInput(audioStream, AudioStreamFormat.GetCompressedFormat(AudioStreamContainerFormat.MULAW));
            var recognizerClient = new SpeechRecognizer(_speechConfig, audioConfig);


            // With the recognizer created, we have access to the speech session ID.
            // It will be used later to notify the right client.
            string sessionId = recognizerClient.Properties.GetProperty(PropertyId.Speech_SessionId);

            recognizerClient.Recognizing += SpeechRecognitionRecognizing;
            recognizerClient.Recognized += SpeechRecognitionRecognized;
            recognizerClient.SessionStarted += SpeechRecognitionSessionStarted;
            recognizerClient.SessionStopped += SpeechRecognitionSessionStopped;
            recognizerClient.Canceled += SpeechRecognitionCanceled;

            // add to a list of connections managed by this class.
            var connection = new AzureSpeechToTextConnection()
            {
                SessionId = sessionId,
                AudioStream = audioStream,
                SpeechClient = recognizerClient,
            };

            _connections.Add(streamId, connection);

            await recognizerClient.StartContinuousRecognitionAsync();

            // Waits for completion.
            // Use Task.WaitAny to keep the task rooted.
            Task.WaitAny(new[] { _stopRecognition.Task });

            // Stops recognition.
            await recognizerClient.StopContinuousRecognitionAsync().ConfigureAwait(false);

        }

        public static void PublishAudioChunkForTranscription(string streamId, byte[] audioChunk)
        {
            /*Console.WriteLine($"publishing to azure from {streamId} - {audioChunk.Length} bytes");*/
            _connections[streamId].AudioStream.Write(audioChunk, 0, audioChunk.Length);
        }

        public async Task SendTranscript(string text, string sessionId)
        {
            var connection = _connections.Where(c => c.Value.SessionId == sessionId).FirstOrDefault();
            Console.WriteLine(text);
            
        }

        public static async Task Disconnect(string streamId)
        {
            var connection = _connections[streamId];
            await connection.SpeechClient.StopContinuousRecognitionAsync();
            connection.SpeechClient.Dispose();
            connection.AudioStream.Dispose();
            _connections.Remove(streamId);
        }

        private static void SpeechRecognitionCanceled(object sender, SpeechRecognitionCanceledEventArgs e)
        {
            Console.WriteLine($"CANCELED: Reason={e.Reason}");

            if (e.Reason == CancellationReason.Error)
            {
                Console.WriteLine($"CANCELED: ErrorCode={e.ErrorCode}");
                Console.WriteLine($"CANCELED: ErrorDetails={e.ErrorDetails}");
                Console.WriteLine($"CANCELED: Did you update the subscription info?");
            }

            _stopRecognition.TrySetResult(0);
        }

        private static void SpeechRecognitionSessionStopped(object sender, SessionEventArgs e)
        {
            Console.WriteLine("\nSession stopped event.");
            Console.WriteLine("\nStop recognition.");
            _stopRecognition.TrySetResult(0);
        }

        private static void SpeechRecognitionSessionStarted(object sender, SessionEventArgs e)
        {
            Console.WriteLine("\nSession started event.");
        }

        private static void SpeechRecognitionRecognized(object sender, SpeechRecognitionEventArgs e)
        {
            if (e.Result.Reason == ResultReason.RecognizedSpeech)
            {
                Console.WriteLine($"RECOGNIZED: Text={e.Result.Text}");
            }
            else if (e.Result.Reason == ResultReason.NoMatch)
            {
                Console.WriteLine($"NOMATCH: Speech could not be recognized.");
            }
        }

        private static void SpeechRecognitionRecognizing(object sender, SpeechRecognitionEventArgs e)
        {
            Console.WriteLine($"RECOGNIZING: Text={e.Result.Text}");
        }
    }
}
