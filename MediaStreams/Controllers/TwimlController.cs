using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Twilio.AspNet.Core;
using Twilio.TwiML;
using Twilio.TwiML.Voice;
using Twilio.Types;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MediaStreams.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TwimlController : TwilioController
    {
        // GET: api/<MediaStreamsController>
        [HttpGet]
        [Produces("application/xml")]
        public TwiMLResult Get()
        {
            var voiceResponse = new VoiceResponse();
            var connect = new Connect();
            connect.Stream(url: "wss://jyoung.ngrok.io/api/mediastreamhandler");
            voiceResponse.Append(connect);
            return TwiML(voiceResponse);
        }

        // Post: api/<MediaStreamsController>
        [HttpPost]
        [Produces("application/xml")]
        public TwiMLResult Post()
        {
            Console.WriteLine(Request.Body);

            var voiceResponse = new VoiceResponse();
            var connect = new Connect();
            connect.Stream(url: "wss://jyoung.ngrok.io/api/mediastreamhandler");
            voiceResponse.Append(connect);
            return TwiML(voiceResponse);
        }
    }
}
