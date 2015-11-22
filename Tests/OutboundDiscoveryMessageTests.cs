using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orvibo.Messaging.Outbound;

namespace Tests
{
    [TestClass]
    public sealed class OutboundDiscoveryMessageTests
    {
        [TestMethod]
        public void GetPayload_ShouldBeValid()
        {
            var message = new OutboundDiscoveryMessage();

            var expected = new byte[] { 0x68, 0x64, 0x00, 0x06, 0x71, 0x61 };
            var actual = message.GetPayload();

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
