using System.Text;
using BitfinexClientSharp.Dtos;

namespace BitfinexClientSharp.WSocket.Adapters
{
    public class TradesResponseAdapter : IResponseAdapter
    {
        private readonly IResponseValidator _responseValidator;

        public TradesResponseAdapter(IResponseValidator responsseValidator)
        {
            _responseValidator = responsseValidator;
        }

        public Encoding Encoder { get; set; }

        public IResponse Adapt(Pair pair, byte[] buffer)
        {
            throw new System.NotImplementedException();
        }
    }
}