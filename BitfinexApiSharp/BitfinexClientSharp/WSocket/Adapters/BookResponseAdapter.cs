using BitfinexClientSharp.Dtos;

namespace BitfinexClientSharp.WSocket.Adapters
{
    public class BookResponseAdapter : IResponseAdapter
    {
        public IResponse Adapt(Pair pair, byte[] buffer)
        {
            throw new System.NotImplementedException();
        }
    }
}