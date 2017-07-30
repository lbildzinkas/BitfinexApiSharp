using BitfinexClientSharp.Dtos;

namespace BitfinexClientSharp.WSocket
{
    public interface IResponseAdapterFactory
    {
        IResponseAdapter GetAdapter(ChannelType channel);
    }
}