using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orvibo.Messaging;
using Orvibo.Messaging.Inbound;

namespace Tests
{
    [TestClass]
    public sealed class MessageParserTests
    {
        private static readonly IMessageParser Parser = new MessageParser();

        [TestMethod]
        [ExpectedException(typeof (MessageException))]
        public void Parse_DataLengthLessThan6_ShouldThrowMessageException()
        {
            Parser.Parse(Enumerable.Repeat((byte) 0, 5).ToArray());
        }

        [TestMethod]
        public void Parse_InboundDiscoveryMessageData_ShouldReturnInboundDiscoveryMessage()
        {
            var data = CreateMessageData(42, 0x71, 0x61);
            var message = Parser.Parse(data);

            Assert.IsInstanceOfType(message, typeof (InboundDiscoveryMessage));
        }

        [TestMethod]
        public void Parse_InboundRediscoveryMessageData_ShouldReturnInboundRediscoveryMessage()
        {
            var data = CreateMessageData(42, 0x71, 0x68);
            var message = Parser.Parse(data);

            Assert.IsInstanceOfType(message, typeof (InboundRediscoveryMessage));
        }

        [TestMethod]
        public void Parse_InboundStateChangeMessageData_ShouldReturnInboundStateChangeMessage()
        {
            var data = CreateMessageData(24, 0x73, 0x66);
            var message = Parser.Parse(data);

            Assert.IsInstanceOfType(message, typeof (InboundStateChangeMessage));
        }

        [TestMethod]
        public void Parse_InboundSubscribeMessageData_ShouldReturnInboundDiscoveryMessage()
        {
            var data = CreateMessageData(24, 0x63, 0x6C);
            var message = Parser.Parse(data);

            Assert.IsInstanceOfType(message, typeof (InboundSubscribeMessage));
        }

        [TestMethod]
        [ExpectedException(typeof (MessageException))]
        public void Parse_InvalidCommand_ShouldThrowMessageException()
        {
            Parser.Parse(new byte[] { 0x68, 0x64, 0, 0, 0, 0 });
        }

        [TestMethod]
        [ExpectedException(typeof (MessageException))]
        public void Parse_InvalidKey_ShouldThrowMessageException()
        {
            Parser.Parse(Enumerable.Repeat((byte) 0, 6).ToArray());
        }

        [TestMethod]
        [ExpectedException(typeof (MessageException))]
        public void Parse_KeyNotAtBeginning_ShouldThrowMessageException()
        {
            Parser.Parse(new byte[] { 0, 0x68, 0x64, 0, 0, 0 });
        }

        [TestMethod]
        [ExpectedException(typeof (MessageException))]
        public void Parse_NullData_ShouldThrowMessageException()
        {
            Parser.Parse(null);
        }

        private static byte[] CreateMessageData(int length, byte commandUpper, byte commandLower)
        {
            var data = Enumerable.Repeat((byte) 0, length).ToArray();

            data[0] = 0x68;
            data[1] = 0x64;
            data[2] = 0x00;
            data[3] = (byte) length;
            data[4] = commandUpper;
            data[5] = commandLower;
            data[7] = 0xAC;
            data[8] = 0xCF;

            return data;
        }
    }
}
