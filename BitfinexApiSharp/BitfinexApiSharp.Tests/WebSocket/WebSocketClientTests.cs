using System;
using Xunit;
using WebSocketSharp;
namespace BitfinexApiSharp.Tests.WebSocket
{
    
    public class WebSocketClientTests
    {
        [Fact]
        public void first()
        {
            var serverUrl = "wss://api.bitfinex.com/ws";
            var ws = new WebSocketSharp.WebSocket(serverUrl);

           

            var webSocketClient = new WebSocketApiClient(ws);
        }
    }

    public class WebSocketApiClient
    {
        private readonly WebSocketSharp.WebSocket _webSocket;

        public WebSocketApiClient(WebSocketSharp.WebSocket webSocket)
        {
            _webSocket = webSocket;
        }
    }
}