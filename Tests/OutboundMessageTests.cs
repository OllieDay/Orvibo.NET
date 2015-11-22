using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Orvibo.Messaging.Outbound;

namespace Tests
{
    [TestClass]
    public sealed class OutboundMessageTests
    {
        [TestMethod]
        public void GetPayload_ShouldBeValid()
        {
            var mock = new Mock<OutboundMessage>();
            var message = mock.Object;

            var expected = new byte[] { 0x68, 0x64, 0x00, 0x04 };
            var actual = message.GetPayload();

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
