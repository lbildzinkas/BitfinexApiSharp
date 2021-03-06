﻿using System.Text;
using BitfinexClientSharp.Dtos;

namespace BitfinexClientSharp.WSocket.Adapters
{
    public class BookResponseAdapter : IResponseAdapter
    {
        private readonly IResponseValidator _responseValidator;

        public BookResponseAdapter(IResponseValidator responseValidator)
        {
            _responseValidator = responseValidator;
        }

        public Encoding Encoder { get; set; }

        public IResponse Adapt(Pair pair, byte[] buffer)
        {
            throw new System.NotImplementedException();
        }
    }
}