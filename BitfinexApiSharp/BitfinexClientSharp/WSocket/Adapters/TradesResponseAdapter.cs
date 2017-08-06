using System.Text;
using BitfinexClientSharp.Dtos;

namespace BitfinexClientSharp.WSocket.Adapters
{
    public class TradesResponseAdapter : IResponseAdapter
    {
        public Encoding Encoder { get; set; }

        public IResponse Adapt(Pair pair, byte[] buffer)
        {
            throw new System.NotImplementedException();
        }
    }
}