using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using BitfinexClientSharp.Dtos;

namespace BitfinexClientSharp.WSocket.Adapters
{
    public class WSocketResponseAdapterFactory : IResponseAdapterFactory
    {
        private readonly Dictionary<ChannelType, IResponseAdapter> _factoryDictionary;

        public WSocketResponseAdapterFactory()
        {
            _factoryDictionary = new Dictionary<ChannelType, IResponseAdapter>()
            {
                {ChannelType.Book, new BookResponseAdapter() },
                {ChannelType.Ticker, new TickerResponseAdapter() },
                {ChannelType.Trades, new TradesResponseAdapter() }
            };
        }
        public IResponseAdapter GetAdapter(ChannelType channel)
        {
            IResponseAdapter adapter;
            if(_factoryDictionary.TryGetValue(channel, out adapter))
            {
                return adapter;
            }

            throw new ArgumentException("Channel is not valid!");
        }
    }
}