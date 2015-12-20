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
            var messageSender = Mock.Of<IMessageSender>();
            var broadcastMessageSender = new BroadcastMessageSender(messageSender);

            broadcastMessageSender.SendDiscoveryMessage();

            Mock.Get(messageSender).Verify(ms => ms.Send(It.IsAny<OutboundDiscoveryMessage>(), It.Is<IPAddress>(ip => ip.Equals(IPAddress.Broadcast))));
        }

        [TestMethod]
        public void SendDiscoveryMessage_SentMessage_ShouldBeOutboundDiscoveryMessage()
        {
            var messageSender = Mock.Of<IMessageSender>();
            var broadcastMessageSender = new BroadcastMessageSender(messageSender);

            broadcastMessageSender.SendDiscoveryMessage();

            Mock.Get(messageSender).Verify(ms => ms.Send(It.IsAny<OutboundDiscoveryMessage>(), It.IsAny<IPAddress>()));
        }

        [TestMethod]
        public void SendRediscoveryMessage_DestinationIPAddress_ShouldBeBroadcastAddress()
        {
            var messageSender = Mock.Of<IMessageSender>();
            var broadcastMessageSender = new BroadcastMessageSender(messageSender);

            broadcastMessageSender.SendRediscoveryMessage(PhysicalAddress.None);

            Mock.Get(messageSender).Verify(ms => ms.Send(It.IsAny<OutboundRediscoveryMessage>(), It.Is<IPAddress>(ip => ip.Equals(IPAddress.Broadcast))));
        }

        [TestMethod]
        public void SendRediscoveryMessage_SentMessage_ShouldBeOutboundRediscoveryMessage()
        {
            var messageSender = Mock.Of<IMessageSender>();
            var broadcastMessageSender = new BroadcastMessageSender(messageSender);

            broadcastMessageSender.SendRediscoveryMessage(PhysicalAddress.None);

            Mock.Get(messageSender).Verify(ms => ms.Send(It.IsAny<OutboundRediscoveryMessage>(), It.IsAny<IPAddress>()));
        }
    }
}
