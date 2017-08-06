using System.Text;
using BitfinexClientSharp.Dtos;

namespace BitfinexClientSharp.WSocket.Adapters
{
    public class TickerResponseAdapter : IResponseAdapter
    {
        private readonly IResponseValidator _responseValidator;

        public TickerResponseAdapter(IResponseValidator responseValidator)
        {
            _responseValidator = responseValidator;
        }
        
        public TickerResponseAdapter(IResponseValidator responseValidator, Encoding encoder) : this(responseValidator)
        {
            Encoder = encoder;
        }

        public Encoding Encoder { get; set; }

        public IResponse Adapt(Pair pair, byte[] buffer)
        {
            var responseString = Encoder.GetString(buffer);
            responseString = responseString.Replace("[", string.Empty).Replace("]", string.Empty).Replace("\0", string.Empty);
            var valuesArray = responseString.Split(',');
            IResponse response = null;

            if (!_responseValidator.IsHeaderMsg(responseString))
            {
                response = new TickerResponse()
                {
                    Ask = float.TryParse(valuesArray[TickerArrayPositions.AskPosition], out float ask)
                        ? ask
                        : float.MinValue,
                    AskSize = float.TryParse(valuesArray[TickerArrayPositions.AskSizePosition], out float askSize)
                        ? askSize
                        : float.MinValue,
                    Bid = float.TryParse(valuesArray[TickerArrayPositions.BidSizePosition], out float bid)
                        ? bid
                        : float.MinValue,
                    BidSize = float.TryParse(valuesArray[TickerArrayPositions.BidSizePosition], out float bidSize)
                        ? bidSize
                        : float.MinValue,
                    ChannelId = valuesArray[TickerArrayPositions.ChannelIdPosition] ?? string.Empty,
                    DailyChange = float.TryParse(valuesArray[TickerArrayPositions.DaiyChangePosition],
                        out float dailyChange)
                        ? dailyChange
                        : float.MinValue,
                    DailyChangePercentage =
                        float.TryParse(valuesArray[TickerArrayPositions.DailyChangePercentagePosition],
                            out float dailyChangePercentage)
                            ? dailyChangePercentage
                            : float.MinValue,
                    High = float.TryParse(valuesArray[TickerArrayPositions.HighPosition], out float highPosition)
                        ? highPosition
                        : float.MinValue,
                    LastPrice = float.TryParse(valuesArray[TickerArrayPositions.LastPricePosition], out float lastPrice)
                        ? lastPrice
                        : float.MinValue,
                    Low = float.TryParse(valuesArray[TickerArrayPositions.LowPosition], out float low)
                        ? low
                        : float.MinValue,
                    Pair = pair,
                    Volume = float.TryParse(valuesArray[TickerArrayPositions.VolumePosition], out float volume)
                        ? volume
                        : float.MinValue
                };
            }
            else
            {
                response = new HeaderResponse()
                {
                    Pair = pair,
                    Msg = responseString
                };
            }

            return response;
        }
    }
}