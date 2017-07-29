using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BitfinexClientSharp.WSocket
{
    public class WSocketClient
    {
        private readonly string _serverUrl;
        private readonly ConcurrentDictionary<(CryptoPair c, OperationType o), ClientWebSocket> _wsClients;
        private const int receiveChunkSize = 1024;
        private readonly TimeSpan _tickerDelay;
        private UTF8Encoding encoder = new UTF8Encoding();

        public WSocketClient(string serverUrl, TimeSpan tickerDelay)
        {
            _serverUrl = serverUrl;
            _tickerDelay = tickerDelay;
            _wsClients = new ConcurrentDictionary<(CryptoPair c, OperationType o), ClientWebSocket>();
        }

        public async Task TickerSubscribe(CryptoPair cryptoPair, Action<byte[]> onMessageReceived)
        {
            ClientWebSocket webSocket = null;

            try
            {
                webSocket = new ClientWebSocket();

                if (_wsClients.TryAdd((cryptoPair, OperationType.Ticker), webSocket))
                {
                    await webSocket.ConnectAsync(new Uri(_serverUrl), CancellationToken.None).ConfigureAwait(false);
                    await Task.WhenAll(Receive(webSocket, onMessageReceived),
                        Send(cryptoPair, webSocket, onMessageReceived)).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                //todo: log
            }
            finally
            {
                webSocket?.Dispose();
            }
        }

        private async Task Send(CryptoPair cryptoPair, ClientWebSocket webSocket, Action<byte[]> onMessageReceived)
        {
            var buffer = encoder.GetBytes($"{{\r\n  \"event\": \"subscribe\",\r\n  \"channel\": \"ticker\",\r\n  \"symbol\": \"{Enum.GetName(typeof(CryptoPair), cryptoPair)}\"\r\n}}");
            await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None).ConfigureAwait(false);

            while (webSocket.State == WebSocketState.Open)
            {
                onMessageReceived(buffer);
                await Task.Delay(_tickerDelay);
            }
        }

        private async Task Receive(ClientWebSocket webSocket, Action<byte[]> onMessageReceived)
        {
            var buffer = new byte[receiveChunkSize];
            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None).ConfigureAwait(false);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None).ConfigureAwait(false);
                }
                else
                {
                    onMessageReceived(buffer);
                }
            }
        }
    }
}
