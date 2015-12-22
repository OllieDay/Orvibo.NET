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
        [TestMethod]
        public void SendKeepaliveMessage_Destination_ShouldMatch()
        {
            using (var messageSender = Mock.Of<IMessageSender>())
            {
                using (var unicastMessageSender = new UnicastMessageSender(messageSender))
                {
                    var networkDevice = CreateNetworkDevice();

                    unicastMessageSender.SendKeepaliveMessage(networkDevice);

                    Mock.Get(messageSender).Verify(ms => ms.Send(It.IsAny<IOutboundMessage>(), It.Is<IPAddress>(ip => ip.Equals(networkDevice.IPAddress))));
                }
            }
        }

        [TestMethod]
        public void SendKeepaliveMessage_SentMessage_ShouldBeOutboundKeepaliveMessage()
        {
            using (var messageSender = Mock.Of<IMessageSender>())
            {
                using (var unicastMessageSender = new UnicastMessageSender(messageSender))
                {
                    var networkDevice = CreateNetworkDevice();

                    unicastMessageSender.SendKeepaliveMessage(networkDevice);

                    Mock.Get(messageSender).Verify(ms => ms.Send(It.IsAny<OutboundKeepaliveMessage>(), It.IsAny<IPAddress>()));
                }
            }
        }

        [TestMethod]
        public void SendOffMessage_Destination_ShouldMatch()
        {
            using (var messageSender = Mock.Of<IMessageSender>())
            {
                using (var unicastMessageSender = new UnicastMessageSender(messageSender))
                {
                    var networkDevice = CreateNetworkDevice();

                    unicastMessageSender.SendOffMessage(networkDevice);

                    Mock.Get(messageSender).Verify(ms => ms.Send(It.IsAny<IOutboundMessage>(), It.Is<IPAddress>(ip => ip.Equals(networkDevice.IPAddress))));
                }
            }
        }

        [TestMethod]
        public void SendOffMessage_SentMessage_ShouldBeOutboundStateChangeMessage()
        {
            using (var messageSender = Mock.Of<IMessageSender>())
            {
                using (var unicastMessageSender = new UnicastMessageSender(messageSender))
                {
                    var networkDevice = CreateNetworkDevice();

                    unicastMessageSender.SendOffMessage(networkDevice);

                    Mock.Get(messageSender).Verify(ms => ms.Send(It.IsAny<OutboundStateChangeMessage>(), It.IsAny<IPAddress>()));
                }
            }
        }

        [TestMethod]
        public void SendOffMessage_SentMessage_StateShouldBeOff()
        {
            using (var messageSender = Mock.Of<IMessageSender>())
            {
                using (var unicastMessageSender = new UnicastMessageSender(messageSender))
                {
                    var networkDevice = CreateNetworkDevice();

                    unicastMessageSender.SendOffMessage(networkDevice);

                    Mock.Get(messageSender).Verify(ms => ms.Send(It.Is<IOutboundMessage>(m => m.GetPayload().Last() == 0), It.IsAny<IPAddress>()));
                }
            }
        }

        [TestMethod]
        public void SendOnMessage_Destination_ShouldMatch()
        {
            using (var messageSender = Mock.Of<IMessageSender>())
            {
                using (var unicastMessageSender = new UnicastMessageSender(messageSender))
                {
                    var networkDevice = CreateNetworkDevice();

                    unicastMessageSender.SendOnMessage(networkDevice);

                    Mock.Get(messageSender).Verify(ms => ms.Send(It.IsAny<IOutboundMessage>(), It.Is<IPAddress>(ip => ip.Equals(networkDevice.IPAddress))));
                }
            }
        }

        [TestMethod]
        public void SendOnMessage_SentMessage_ShouldBeOutboundStateChangeMessage()
        {
            using (var messageSender = Mock.Of<IMessageSender>())
            {
                using (var unicastMessageSender = new UnicastMessageSender(messageSender))
                {
                    var networkDevice = CreateNetworkDevice();

                    unicastMessageSender.SendOnMessage(networkDevice);

                    Mock.Get(messageSender).Verify(ms => ms.Send(It.IsAny<OutboundStateChangeMessage>(), It.IsAny<IPAddress>()));
                }
            }
        }

        [TestMethod]
        public void SendOnMessage_SentMessage_StateShouldBeOn()
        {
            using (var messageSender = Mock.Of<IMessageSender>())
            {
                using (var unicastMessageSender = new UnicastMessageSender(messageSender))
                {
                    var networkDevice = CreateNetworkDevice();

                    unicastMessageSender.SendOnMessage(networkDevice);

                    Mock.Get(messageSender).Verify(ms => ms.Send(It.Is<IOutboundMessage>(m => m.GetPayload().Last() == 1), It.IsAny<IPAddress>()));
                }
            }
        }

        [TestMethod]
        public void SendSubscribeMessage_Destination_ShouldMatch()
        {
            using (var messageSender = Mock.Of<IMessageSender>())
            {
                using (var unicastMessageSender = new UnicastMessageSender(messageSender))
                {
                    var networkDevice = CreateNetworkDevice();

                    unicastMessageSender.SendSubscribeMessage(networkDevice);

                    Mock.Get(messageSender).Verify(ms => ms.Send(It.IsAny<IOutboundMessage>(), It.Is<IPAddress>(ip => ip.Equals(networkDevice.IPAddress))));
                }
            }
        }

        [TestMethod]
        public void SendSubscribeMessage_SentMessage_ShouldBeOutboundSubscribeMessage()
        {
            using (var messageSender = Mock.Of<IMessageSender>())
            {
                using (var unicastMessageSender = new UnicastMessageSender(messageSender))
                {
                    var networkDevice = CreateNetworkDevice();

                    unicastMessageSender.SendSubscribeMessage(networkDevice);

                    Mock.Get(messageSender).Verify(ms => ms.Send(It.IsAny<OutboundSubscribeMessage>(), It.IsAny<IPAddress>()));
                }
            }
        }

        private INetworkDevice CreateNetworkDevice()
        {
            var networkDeviceMock = new Mock<INetworkDevice>();
            networkDeviceMock.Setup(nd => nd.MacAddress).Returns(PhysicalAddress.None);
            networkDeviceMock.Setup(nd => nd.IPAddress).Returns(IPAddress.Loopback);

            return networkDeviceMock.Object;
        }
    }
}
