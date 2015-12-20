using System;
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
            AssertInboundMessageOfType(42, 0x71, 0x61, typeof (InboundDiscoveryMessage));
        }

        [TestMethod]
        public void Parse_InboundKeepaliveMessageData_ShouldReturnInboundKeepaliveMessage()
        {
            AssertInboundMessageOfType(23, 0x68, 0x62, typeof (InboundKeepaliveMessage));
        }

        [TestMethod]
        public void Parse_InboundRediscoveryMessageData_ShouldReturnInboundRediscoveryMessage()
        {
            AssertInboundMessageOfType(42, 0x71, 0x68, typeof (InboundRediscoveryMessage));
        }

        [TestMethod]
        public void Parse_InboundStateChangeMessageData_ShouldReturnInboundStateChangeMessage()
        {
            AssertInboundMessageOfType(24, 0x73, 0x66, typeof (InboundStateChangeMessage));
        }

        [TestMethod]
        public void Parse_InboundSubscribeMessageData_ShouldReturnInboundDiscoveryMessage()
        {
            AssertInboundMessageOfType(24, 0x63, 0x6C, typeof (InboundSubscribeMessage));
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

        private static byte[] CreateMessageData(byte length, byte commandUpper, byte commandLower)
        {
            var data = Enumerable.Repeat((byte) 0, length).ToArray();

            data[0] = 0x68;
            data[1] = 0x64;
            data[2] = 0x00;
            data[3] = length;
            data[4] = commandUpper;
            data[5] = commandLower;
            data[7] = 0xAC;
            data[8] = 0xCF;

            return data;
        }

        private void AssertInboundMessageOfType(byte length, byte commandUpper, byte commandLower, Type expectedType)
        {
            var data = CreateMessageData(length, commandUpper, commandLower);
            var message = Parser.Parse(data);

            Assert.IsInstanceOfType(message, expectedType);
        }
    }
}
