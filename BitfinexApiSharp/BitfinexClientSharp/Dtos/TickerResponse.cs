namespace BitfinexClientSharp.Dtos
{
    public class TickerResponse : IResponse
    {
        public Pair Pair { get; set; }
        public string ChannelId { get; set; }
        public float Bid { get; set; }
        public float BidSize { get; set; }
        public float Ask { get; set; }
        public float AskSize { get; set; }
        public float DailyChange { get; set; }
        public float DailyChangePercentage { get; set; }
        public float LastPrice { get; set; }
        public float Volume { get; set; }
        public float High { get; set; }
        public float Low { get; set; }
    }
}