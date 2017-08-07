namespace BitfinexClientSharp.Dtos
{
    public class TickerResponse : IResponse
    {
        public Pair Pair { get; set; }
        public string ChannelId { get; set; }
        public decimal Bid { get; set; }
        public decimal BidSize { get; set; }
        public decimal Ask { get; set; }
        public decimal AskSize { get; set; }
        public decimal DailyChange { get; set; }
        public decimal DailyChangePercentage { get; set; }
        public decimal LastPrice { get; set; }
        public decimal Volume { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
    }
}