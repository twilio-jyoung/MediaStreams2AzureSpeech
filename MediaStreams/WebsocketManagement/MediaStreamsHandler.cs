using MediaStreams.Models;
using MediaStreams.SpeechToText;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace MediaStreams.WebsocketManagement
{
    public class MediaStreamsHandler : WebSocketHandler
    {
        private ConnectionManager _webSocketConnectionManager;
        private bool _writeAudioInputToConsole = true;

        public MediaStreamsHandler(ConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager)
        {
            _webSocketConnectionManager = webSocketConnectionManager;
        }
        
        // websocket connection events
        public override async Task OnConnected(WebSocket socket)
        {
            await base.OnConnected(socket);

            var socketId = WebSocketConnectionManager.GetId(socket);
            await SendMessageToAllAsync($"{socketId} is now connected");
        }

        public override async Task OnDisconnected(WebSocket socket)
        {
            // remove connection from azure and kill the sessions
            await AzureSpeechRecognizer.Disconnect(WebSocketConnectionManager.GetId(socket));
           
            await base.OnDisconnected(socket);
        }


        // websocket transferring data
        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var socketId = WebSocketConnectionManager.GetId(socket);
            var content = Encoding.UTF8.GetString(buffer, 0, result.Count);

            var chunk = JsonConvert.DeserializeObject<MediaStreamContent>(content);

            if (chunk.Event == "connected")
                Console.WriteLine($"Stream Connected: {chunk.Protocol}");

            if (chunk.Event == "start")
            {
                Console.WriteLine($"Stream Started: {chunk.Start.AccountSid} - {chunk.Start.CallSid} - {chunk.StreamSid} | Format: {chunk.Start.MediaFormat.ToString()}");
                await AzureSpeechRecognizer.AudioStart(chunk.StreamSid);
            }
                
            if (chunk.Event == "media")
            {
                // Console.WriteLine($"Media Detected: {chunk.StreamSid} - {chunk.SequenceNumber} - {chunk.Media.Payload}");
                // if(_writeAudioInputToConsole)
                //      Console.WriteLine($"Media Detected: {chunk.StreamSid} - {chunk.SequenceNumber} - {chunk.Media.Payload}");
                var media = Convert.FromBase64String(chunk.Media.Payload);

                Console.WriteLine(string.Join(" ", media.Select(x => Convert.ToString(x, 2).PadLeft(8, '0'))));

                // pass this payload to azure speech to text
                AzureSpeechRecognizer.PublishAudioChunkForTranscription(chunk.StreamSid, media);
            }
                
            if (chunk.Event == "stop")
                Console.WriteLine($"Stream Stopped: {chunk.Stop.AccountSid} - {chunk.Stop.CallSid} - {chunk.StreamSid}");
            
                

            var message = $"{socketId} said: {Encoding.UTF8.GetString(buffer, 0, result.Count)}";



            //Console.WriteLine(message);

            await SendMessageToAllAsync(message);
        }


    }
}