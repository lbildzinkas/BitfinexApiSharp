using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using BitfinexClientSharp.Dtos;

namespace BitfinexClientSharp.WSocket.Adapters
{
    public class WSocketResponseAdapterFactory : IResponseAdapterFactory
    {
        private readonly Dictionary<ChannelType, IResponseAdapter> _factoryDictionary;

        public WSocketResponseAdapterFactory(IResponseValidator responseValidator)
        {
            _factoryDictionary = new Dictionary<ChannelType, IResponseAdapter>()
            {
                {ChannelType.Book, new BookResponseAdapter(responseValidator)},
                {ChannelType.Ticker, new TickerResponseAdapter(responseValidator) },
                {ChannelType.Trades, new TradesResponseAdapter(responseValidator) }
            };
        }

        public IResponseAdapter GetAdapter(ChannelType channel, Encoding encoder)
        {
            IResponseAdapter adapter;
            if(_factoryDictionary.TryGetValue(channel, out adapter))
            {
                adapter.Encoder = encoder;
                return adapter;
            }

            throw new ArgumentException("Channel is not valid!");
        }
    }
}