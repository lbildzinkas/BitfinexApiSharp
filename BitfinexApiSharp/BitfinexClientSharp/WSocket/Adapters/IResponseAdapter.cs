using System.Text;
using BitfinexClientSharp.Dtos;

namespace BitfinexClientSharp.WSocket.Adapters
{
    public interface IResponseAdapter
    {
        Encoding Encoder { get; set; }
        IResponse Adapt(Pair pair, byte[] buffer);
    }
}