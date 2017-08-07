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
                    Ask = decimal.TryParse(valuesArray[TickerArrayPositions.AskPosition], out decimal ask)
                        ? ask
                        : decimal.MinValue,
                    AskSize = decimal.TryParse(valuesArray[TickerArrayPositions.AskSizePosition], out decimal askSize)
                        ? askSize
                        : decimal.MinValue,
                    Bid = decimal.TryParse(valuesArray[TickerArrayPositions.BidSizePosition], out decimal bid)
                        ? bid
                        : decimal.MinValue,
                    BidSize = decimal.TryParse(valuesArray[TickerArrayPositions.BidSizePosition], out decimal bidSize)
                        ? bidSize
                        : decimal.MinValue,
                    ChannelId = valuesArray[TickerArrayPositions.ChannelIdPosition] ?? string.Empty,
                    DailyChange = decimal.TryParse(valuesArray[TickerArrayPositions.DaiyChangePosition],
                        out decimal dailyChange)
                        ? dailyChange
                        : decimal.MinValue,
                    DailyChangePercentage =
                        decimal.TryParse(valuesArray[TickerArrayPositions.DailyChangePercentagePosition],
                            out decimal dailyChangePercentage)
                            ? dailyChangePercentage
                            : decimal.MinValue,
                    High = decimal.TryParse(valuesArray[TickerArrayPositions.HighPosition], out decimal highPosition)
                        ? highPosition
                        : decimal.MinValue,
                    LastPrice = decimal.TryParse(valuesArray[TickerArrayPositions.LastPricePosition], out decimal lastPrice)
                        ? lastPrice
                        : decimal.MinValue,
                    Low = decimal.TryParse(valuesArray[TickerArrayPositions.LowPosition], out decimal low)
                        ? low
                        : decimal.MinValue,
                    Pair = pair,
                    Volume = decimal.TryParse(valuesArray[TickerArrayPositions.VolumePosition], out decimal volume)
                        ? volume
                        : decimal.MinValue
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