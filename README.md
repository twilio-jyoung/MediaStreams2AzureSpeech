# MediaStreams2AzureSpeech

***this sample is not yet working***

## Setup
 - Install gstreamer (https://gstreamer.freedesktop.org/download/)
 - Run the application, copy the host and port
 - Stop the application
 - launch ngrok
 - - ngrok http https://localhost/<port> -host-header="localhost/<port> -subdomain="<subdomain>"
 - update the connect.Stream url on TwimlController.cs to point to your ngrok url
 - buy a phone number and point the voice url to your ngrok url + /api/twiml
 - - ex. https://jyoung.ngrok.io/api/twiml
 - Start the application again
 - Show output from "Media Streams - ASP.NET Core Web Server
 - make sure you can hit the twiml ngrok url in the browser
 - - ex. https://jyoung.ngrok.io/api/twiml
 - Place a short call into your phone number and beggin speaking
 - See error in output logs.
