using BitfinexClientSharp.Dtos;

namespace BitfinexClientSharp.WSocket
{
    public interface IResponseAdapter
    {
        IResponse Adapt(Pair pair, byte[] buffer);
    }
}