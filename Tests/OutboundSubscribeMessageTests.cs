using System.Net.NetworkInformation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orvibo.Messaging.Outbound;

namespace Tests
{
    [TestClass]
    public sealed class OutboundSubscribeMessageTests
    {
        [TestMethod]
        public void GetPayload_Data_ShouldBeValid()
        {
            var macAddress = new PhysicalAddress(new byte[] { 0, 0, 0, 0, 0, 0 });
            var message = new OutboundSubscribeMessage(macAddress);

            var expected = new byte[]
            {
                0x68,
                0x64,
                0x00,
                0x1E,
                0x63,
                0x6C,
                0,
                0,
                0,
                0,
                0,
                0,
                0x20,
                0x20,
                0x20,
                0x20,
                0x20,
                0x20,
                0,
                0,
                0,
                0,
                0,
                0,
                0x20,
                0x20,
                0x20,
                0x20,
                0x20,
                0x20
            };

            var actual = message.GetPayload();

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
