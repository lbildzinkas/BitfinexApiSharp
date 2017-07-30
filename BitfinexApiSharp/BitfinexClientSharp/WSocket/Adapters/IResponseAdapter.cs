using BitfinexClientSharp.Dtos;

namespace BitfinexClientSharp.WSocket.Adapters
{
    public interface IResponseAdapter
    {
        IResponse Adapt(Pair pair, byte[] buffer);
    }
}