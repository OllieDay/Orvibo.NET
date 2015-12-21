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
            CreateMessage(SocketState.Unknown);
        }

        [TestMethod]
        public void GetPayload_StateIsOff_ShouldBeValid()
        {
            var message = CreateMessage(SocketState.Off);

            var expected = new byte[] { 0x68, 0x64, 0x00, 0x17, 0x64, 0x63, 0, 0, 0, 0, 0, 0, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x00, 0x00, 0x00, 0x00, 0 };
            var actual = message.GetPayload();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetPayload_StateIsOn_ShouldBeValid()
        {
            var message = CreateMessage(SocketState.On);

            var expected = new byte[] { 0x68, 0x64, 0x00, 0x17, 0x64, 0x63, 0, 0, 0, 0, 0, 0, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x00, 0x00, 0x00, 0x00, 1 };
            var actual = message.GetPayload();

            CollectionAssert.AreEqual(expected, actual);
        }

        private OutboundStateChangeMessage CreateMessage(SocketState state)
        {
            var macAddress = new PhysicalAddress(new byte[] { 0, 0, 0, 0, 0, 0 });

            return new OutboundStateChangeMessage(macAddress, state);
        }
    }
}
