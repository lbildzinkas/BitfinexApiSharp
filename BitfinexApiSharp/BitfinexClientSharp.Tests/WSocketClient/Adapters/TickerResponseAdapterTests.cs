using System.Text;
using BitfinexClientSharp.Dtos;
using BitfinexClientSharp.Tests.Messages;
using BitfinexClientSharp.WSocket.Adapters;
using Moq;
using Xunit;

namespace BitfinexClientSharp.Tests.Adapters
{
    public class TickerResponseAdapterTests
    {
        [Fact]
        public void TickerMessageShouldReturnAdaptedTickerResponse()
        {
            var msg = TickerMessages.TickerMsg;
            var encoderMock = new Mock<Encoding>();
            encoderMock.Setup(s => s.GetString(It.IsAny<byte[]>())).Returns(() => msg);
            var responseValidator = new Mock<IResponseValidator>();
            responseValidator.Setup(v => v.IsHeaderMsg(It.IsAny<string>())).Returns(false);

            var sut = new TickerResponseAdapter(responseValidator.Object, encoderMock.Object);
            var output = sut.Adapt(It.IsAny<Pair>(), It.IsAny<byte[]>());

            Assert.True(output is TickerResponse);
            Assert.True(output.Pair == Pair.BTCUSD);
        }

        [Fact]
        public void HeaderMessageShouldReturnHeaderResponse()
        {
            var msg = GeneralMessages.SubscribeMessage;
            var encoderMock = new Mock<Encoding>();
            encoderMock.Setup(s => s.GetString(It.IsAny<byte[]>())).Returns(() => msg);
            var responseValidator = new Mock<IResponseValidator>();
            responseValidator.Setup(v => v.IsHeaderMsg(It.IsAny<string>())).Returns(true);

            var sut = new TickerResponseAdapter(responseValidator.Object, encoderMock.Object);
            var output = sut.Adapt(It.IsAny<Pair>(), It.IsAny<byte[]>());

            Assert.True(output is HeaderResponse);
            Assert.True(output.Pair == Pair.BTCUSD);
        }
    }
}