namespace BitfinexClientSharp.WSocket.Adapters
{
    public class ResponseValidator : IResponseValidator
    {
        public bool IsHeaderMsg(string msg)
        {
            return msg.Contains("subscribe") || msg.Contains("info");
        }
    }
}