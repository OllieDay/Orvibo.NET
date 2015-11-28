using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Orvibo;
using Orvibo.Messaging.Outbound;

namespace Tests
{
    [TestClass]
    public sealed class UnicastMessageSenderTests
    {
        private static readonly Mock<IMessageSender> Mock = new Mock<IMessageSender>();

        private static readonly IUnicastMessageSender UnicastMessageSender = new UnicastMessageSender(Mock.Object);

        [TestMethod]
        public void SendOffMessage_Destination_ShouldMatch()
        {
            var networkDevice = CreateNetworkDevice();

            UnicastMessageSender.SendOffMessage(networkDevice);

            Mock.Verify(ms => ms.Send(It.IsAny<IOutboundMessage>(), It.Is<IPAddress>(ip => ip.Equals(networkDevice.IPAddress))));
        }

        [TestMethod]
        public void SendOffMessage_SentMessage_ShouldBeOutboundStateChangeMessage()
        {
            var networkDevice = CreateNetworkDevice();

            UnicastMessageSender.SendOffMessage(networkDevice);

            Mock.Verify(ms => ms.Send(It.IsAny<OutboundStateChangeMessage>(), It.IsAny<IPAddress>()));
        }

        [TestMethod]
        public void SendOffMessage_SentMessage_StateShouldBeOff()
        {
            var networkDevice = CreateNetworkDevice();

            UnicastMessageSender.SendOffMessage(networkDevice);

            Mock.Verify(ms => ms.Send(It.Is<OutboundStateChangeMessage>(m => m.GetPayload().Last() == 0), It.IsAny<IPAddress>()));
        }

        [TestMethod]
        public void SendOnMessage_Destination_ShouldMatch()
        {
            var networkDevice = CreateNetworkDevice();

            UnicastMessageSender.SendOnMessage(networkDevice);

            Mock.Verify(ms => ms.Send(It.IsAny<IOutboundMessage>(), It.Is<IPAddress>(ip => ip.Equals(networkDevice.IPAddress))));
        }

        [TestMethod]
        public void SendOnMessage_SentMessage_ShouldBeOutboundStateChangeMessage()
        {
            var networkDevice = CreateNetworkDevice();

            UnicastMessageSender.SendOnMessage(networkDevice);

            Mock.Verify(ms => ms.Send(It.IsAny<OutboundStateChangeMessage>(), It.IsAny<IPAddress>()));
        }

        [TestMethod]
        public void SendOnMessage_SentMessage_StateShouldBeOn()
        {
            var networkDevice = CreateNetworkDevice();

            UnicastMessageSender.SendOnMessage(networkDevice);

            Mock.Verify(ms => ms.Send(It.Is<OutboundStateChangeMessage>(m => m.GetPayload().Last() == 1), It.IsAny<IPAddress>()));
        }

        [TestMethod]
        public void SendSubscribeMessage_Destination_ShouldMatch()
        {
            var networkDevice = CreateNetworkDevice();

            UnicastMessageSender.SendSubscribeMessage(networkDevice);

            Mock.Verify(ms => ms.Send(It.IsAny<IOutboundMessage>(), It.Is<IPAddress>(ip => ip.Equals(networkDevice.IPAddress))));
        }

        [TestMethod]
        public void SendSubscribeMessage_SentMessage_ShouldBeOutboundSubscribeMessage()
        {
            var networkDevice = CreateNetworkDevice();

            UnicastMessageSender.SendSubscribeMessage(networkDevice);

            Mock.Verify(ms => ms.Send(It.IsAny<OutboundSubscribeMessage>(), It.IsAny<IPAddress>()));
        }

        private static INetworkDevice CreateNetworkDevice()
        {
            var networkDeviceMock = new Mock<INetworkDevice>();
            networkDeviceMock.Setup(nd => nd.MacAddress).Returns(PhysicalAddress.None);
            networkDeviceMock.Setup(nd => nd.IPAddress).Returns(IPAddress.Loopback);

            return networkDeviceMock.Object;
        }
    }
}
