using System;
using System.Net.NetworkInformation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orvibo;
using Orvibo.Messaging.Outbound;

namespace Tests
{
    [TestClass]
    public sealed class OutboundStateChangeMessageTests
    {
        [TestMethod]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public void CreateNewOutboundStateChangeMessage_StateIsUnknown_ShouldThrowArgumentOutOfRangeException()
        {
            new OutboundStateChangeMessage(PhysicalAddress.None, SocketState.Unknown);
        }

        [TestMethod]
        public void GetPayload_StateIsOff_ShouldBeValid()
        {
            var macAddress = new PhysicalAddress(new byte[] { 0, 0, 0, 0, 0, 0 });
            var message = new OutboundStateChangeMessage(macAddress, SocketState.Off);

            var expected = new byte[] { 0x68, 0x64, 0x00, 0x17, 0x64, 0x63, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0, 0, 0, 0, 0, 0, 0x00, 0x00, 0x00, 0x00, 0 };
            var actual = message.GetPayload();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetPayload_StateIsOn_ShouldBeValid()
        {
            var macAddress = new PhysicalAddress(new byte[] { 0, 0, 0, 0, 0, 0 });
            var message = new OutboundStateChangeMessage(macAddress, SocketState.On);

            var expected = new byte[] { 0x68, 0x64, 0x00, 0x17, 0x64, 0x63, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0, 0, 0, 0, 0, 0, 0x00, 0x00, 0x00, 0x00, 1 };
            var actual = message.GetPayload();

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
