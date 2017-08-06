namespace BitfinexClientSharp.Dtos
{
    public class HeaderResponse : IResponse
    {
        public Pair Pair { get; set; }
        public string Msg { get; set; }
    }
}