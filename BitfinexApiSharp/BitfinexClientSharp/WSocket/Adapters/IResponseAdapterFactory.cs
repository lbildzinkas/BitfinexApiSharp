using System.Text;
using BitfinexClientSharp.Dtos;

namespace BitfinexClientSharp.WSocket.Adapters
{
    public interface IResponseAdapterFactory
    {
        IResponseAdapter GetAdapter(ChannelType channel, Encoding encoder);
    }
}