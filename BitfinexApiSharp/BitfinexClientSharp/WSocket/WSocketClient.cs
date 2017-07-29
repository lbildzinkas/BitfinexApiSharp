using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BitfinexClientSharp.Dtos;
using Newtonsoft.Json;

namespace BitfinexClientSharp.WSocket
{
    public class WSocketClient
    {
        private readonly string _serverUrl;
        private readonly ConcurrentDictionary<(Pair c, ChannelType o), ClientWebSocket> _wsClients;
        private int _receiveChunkSize = 1024;
        private readonly TimeSpan _tickerDelay;
        private readonly UTF8Encoding encoder = new UTF8Encoding();

        public WSocketClient(string serverUrl, TimeSpan tickerDelay)
        {
            _serverUrl = serverUrl;
            _tickerDelay = tickerDelay;
            _wsClients = new ConcurrentDictionary<(Pair c, ChannelType o), ClientWebSocket>();
        }

        public async Task TickerSubscribe(Pair pair, Action<byte[]> onMessageReceived)
        {
            await Subscribe(ChannelType.Ticker, pair, onMessageReceived);
        }

        public async Task TradeSubscribe(Pair pair, Action<byte[]> onMessageReceived)
        {
            await Subscribe(ChannelType.Trades, pair, onMessageReceived);
        }

        public async Task BookSubscribe(Pair pair, Action<byte[]> onMessageReceived)
        {
            await Subscribe(ChannelType.Book, pair, onMessageReceived);
        }

        private async Task Subscribe(ChannelType type, Pair pair, Action<byte[]> onMessageReceived)
        {
            ClientWebSocket webSocket = null;

            try
            {
                webSocket = new ClientWebSocket();

                if (_wsClients.TryAdd((pair, type), webSocket))
                {
                    await webSocket.ConnectAsync(new Uri(_serverUrl), CancellationToken.None).ConfigureAwait(false);
                    await Task.WhenAll(Receive(webSocket, onMessageReceived),
                        Send(type, pair, webSocket, onMessageReceived)).ConfigureAwait(false);
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

        private async Task Send(ChannelType channel, Pair pair, ClientWebSocket webSocket, Action<byte[]> onMessageReceived)
        {
            var request = new Request(){Event = EventType.subscribe,  Channel = channel, Pair = pair };
            var jsonRequest = JsonConvert.SerializeObject(request);
            var buffer = encoder.GetBytes(jsonRequest);
            await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None).ConfigureAwait(false);

            while (webSocket.State == WebSocketState.Open)
            {
                onMessageReceived(buffer);
                await Task.Delay(_tickerDelay);
            }
        }

        private async Task Receive(ClientWebSocket webSocket, Action<byte[]> onMessageReceived)
        {
            var buffer = new byte[_receiveChunkSize];
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
