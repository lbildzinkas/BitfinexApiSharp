using BitfinexClientSharp.Tests.Messages;
using BitfinexClientSharp.WSocket.Adapters;
using Xunit;

namespace BitfinexClientSharp.Tests.WSocketClient.Adapters
{
    public class ResponseValidatorTests
    {
        [Fact]
        public void InfoHeaderMsgShouldBeValidatedAsHeaderMsg()
        {
            var msg = GeneralMessages.HeaderInfoMessage;

            var sut = new ResponseValidator();

            var output = sut.IsHeaderMsg(msg);

            Assert.True(output);
        }

        [Fact]
        public void SubscribeHeaderMsgShouldBeValidatedAsHeaderMsg()
        {
            var msg = GeneralMessages.SubscribeMessage;

            var sut = new ResponseValidator();

            var output = sut.IsHeaderMsg(msg);

            Assert.True(output);
        }

        [Fact]
        public void SubscribedHeaderMsgShouldBeValidatedAsHeaderMsg()
        {
            var msg = GeneralMessages.SubscribedMessage;

            var sut = new ResponseValidator();

            var output = sut.IsHeaderMsg(msg);

            Assert.True(output);
        }

        [Fact]
        public void MsgShouldNotBeValidatedAsHeaderMsg()
        {
            var msg = TickerMessages.TickerMsg;

            var sut = new ResponseValidator();

            var output = sut.IsHeaderMsg(msg);

            Assert.False(output);
        }
    }
}