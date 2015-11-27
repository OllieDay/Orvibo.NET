using System.Net;
using System.Net.NetworkInformation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Orvibo.Messaging.Outbound;

namespace Tests
{
    [TestClass]
    public sealed class BroadcastMessageSenderTests
    {
        [TestMethod]
        public void SendDiscoveryMessage_DestinationIPAddress_ShouldBeBroadcastAddress()
        {
            var mock = new Mock<IMessageSender>();
            var messageSender = mock.Object;
            var broadcastMessageSender = new BroadcastMessageSender(messageSender);

            broadcastMessageSender.SendDiscoveryMessage();

            mock.Verify(ms => ms.Send(It.IsAny<OutboundDiscoveryMessage>(), It.Is<IPAddress>(ip => ip.Equals(IPAddress.Broadcast))));
        }

        [TestMethod]
        public void SendDiscoveryMessage_SentMessage_ShouldBeOutboundDiscoveryMessage()
        {
            var mock = new Mock<IMessageSender>();
            var messageSender = mock.Object;
            var broadcastMessageSender = new BroadcastMessageSender(messageSender);

            broadcastMessageSender.SendDiscoveryMessage();

            mock.Verify(ms => ms.Send(It.IsAny<OutboundDiscoveryMessage>(), It.IsAny<IPAddress>()));
        }

        [TestMethod]
        public void SendRediscoveryMessage_DestinationIPAddress_ShouldBeBroadcastAddress()
        {
            var mock = new Mock<IMessageSender>();
            var messageSender = mock.Object;
            var broadcastMessageSender = new BroadcastMessageSender(messageSender);

            broadcastMessageSender.SendRediscoveryMessage(PhysicalAddress.None);

            mock.Verify(ms => ms.Send(It.IsAny<OutboundRediscoveryMessage>(), It.Is<IPAddress>(ip => ip.Equals(IPAddress.Broadcast))));
        }

        [TestMethod]
        public void SendRediscoveryMessage_SentMessage_ShouldBeOutboundRediscoveryMessage()
        {
            var mock = new Mock<IMessageSender>();
            var messageSender = mock.Object;
            var broadcastMessageSender = new BroadcastMessageSender(messageSender);

            broadcastMessageSender.SendRediscoveryMessage(PhysicalAddress.None);

            mock.Verify(ms => ms.Send(It.IsAny<OutboundRediscoveryMessage>(), It.IsAny<IPAddress>()));
        }
    }
}
