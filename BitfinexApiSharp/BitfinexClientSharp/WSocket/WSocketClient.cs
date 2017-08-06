using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BitfinexClientSharp.Dtos;
using BitfinexClientSharp.WSocket.Adapters;
using Newtonsoft.Json;

namespace BitfinexClientSharp.WSocket
{
    public class WSocketClient
    {
        private readonly IResponseAdapterFactory _adapterFactory;
        private readonly string _serverUrl;
        private readonly ConcurrentDictionary<(Pair c, ChannelType o), ClientWebSocket> _wsClients;
        private int _receiveChunkSize = 1024;
        private readonly TimeSpan _tickerDelay;
        private readonly Encoding _encoder = new UTF8Encoding();
        
        public WSocketClient(IResponseAdapterFactory adapterFactory, string serverUrl, TimeSpan tickerDelay)
        {
            _adapterFactory = adapterFactory;
            _serverUrl = serverUrl;
            _tickerDelay = tickerDelay;
        }
        
        public WSocketClient(IResponseAdapterFactory adapterFactory, string serverUrl, TimeSpan tickerDelay, Encoding encoder) 
            : this(adapterFactory, serverUrl, tickerDelay)
        {
            _encoder = encoder;
        }

        public async Task TickerSubscribe(Pair pair, Action<IResponse> onMessageReceived)
        {
            await Subscribe(ChannelType.Ticker, pair, onMessageReceived);
        }

        public async Task TradeSubscribe(Pair pair, Action<IResponse> onMessageReceived)
        {
            await Subscribe(ChannelType.Trades, pair, onMessageReceived);
        }

        public async Task BookSubscribe(Pair pair, Action<IResponse> onMessageReceived)
        {
            await Subscribe(ChannelType.Book, pair, onMessageReceived);
        }

        private async Task Subscribe(ChannelType channel, Pair pair, Action<IResponse> onMessageReceived)
        {
            //todo: create an adapter to remove this dependency from web socket client
            ClientWebSocket webSocket = null;

            try
            {
                webSocket = new ClientWebSocket();

                if (_wsClients.TryAdd((pair, channel), webSocket))
                {
                    var responseAdapter = _adapterFactory.GetAdapter(channel, _encoder); 
                    await webSocket.ConnectAsync(new Uri(_serverUrl), CancellationToken.None).ConfigureAwait(false);
                    await Task.WhenAll(Receive(pair, webSocket, onMessageReceived, responseAdapter),
                        Send(channel, pair, webSocket, onMessageReceived, responseAdapter)).ConfigureAwait(false);
                }
            }
            finally
            {
                webSocket?.Dispose();
            }
        }

        private async Task Send(ChannelType channel, Pair pair, ClientWebSocket webSocket, Action<IResponse> onMessageReceived, IResponseAdapter responseAdapter)
        {
            var request = new Request(){Event = EventType.subscribe,  Channel = channel, Pair = pair };
            var jsonRequest = JsonConvert.SerializeObject(request);
            var buffer = _encoder.GetBytes(jsonRequest);
            await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None).ConfigureAwait(false);

            while (webSocket.State == WebSocketState.Open)
            {
                onMessageReceived(responseAdapter.Adapt(pair, buffer));
                await Task.Delay(_tickerDelay);
            }
        }

        private async Task Receive(Pair pair, ClientWebSocket webSocket, Action<IResponse> onMessageReceived, IResponseAdapter responseAdapter)
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
                    onMessageReceived(responseAdapter.Adapt(pair, buffer));
                }
            }
        }
    }
}
