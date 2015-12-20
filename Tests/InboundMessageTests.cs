using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orvibo;
using Orvibo.Messaging;
using Orvibo.Messaging.Inbound;

namespace Tests
{
    [TestClass]
    public sealed class InboundMessageTests
    {
        [TestMethod]
        [ExpectedException(typeof (MessageException))]
        public void CreateNewInboundMessage_DataLengthLessThan6_ShouldThrowMessageException()
        {
            new InboundDiscoveryMessage(new byte[5]);
        }

        [TestMethod]
        [ExpectedException(typeof (MessageException))]
        public void CreateNewInboundMessage_IncompleteMacAddress_ShouldThrowMessageException()
        {
            var data = Enumerable.Repeat((byte) 0, 42).ToArray();
            data[40] = 0xAC;
            data[41] = 0xCF;

            new InboundDiscoveryMessage(data);
        }

        [TestMethod]
        [ExpectedException(typeof (MessageException))]
        public void CreateNewInboundMessage_InvalidMacAddress_ShouldThrowMessageException()
        {
            var data = Enumerable.Repeat((byte) 0, 42).ToArray();

            new InboundDiscoveryMessage(data);
        }

        [TestMethod]
        public void CreateNewInboundMessage_StateIsOff_StateShouldBeOff()
        {
            var data = CreateMessageData();
            data[41] = 0x00;

            var message = new InboundDiscoveryMessage(data);

            var expected = SocketState.Off;
            var actual = message.State;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CreateNewInboundMessage_StateIsOn_StateShouldBeOn()
        {
            var data = CreateMessageData();
            data[41] = 0x01;

            var message = new InboundDiscoveryMessage(data);

            var expected = SocketState.On;
            var actual = message.State;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof (MessageException))]
        public void CreateNewInboundMessage_StateIsUnknown_ShouldThrowMessageException()
        {
            var data = CreateMessageData();
            data[41] = 0xFF;

            new InboundDiscoveryMessage(data);
        }

        private byte[] CreateMessageData()
        {
            var data = Enumerable.Repeat((byte) 0, 42).ToArray();

            data[7] = 0xAC;
            data[8] = 0xCF;

            return data;
        }
    }
}
