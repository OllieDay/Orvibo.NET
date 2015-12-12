using System.Net;
using System.Net.NetworkInformation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Orvibo;
using Orvibo.Messaging.Inbound;
using Orvibo.Messaging.Outbound;

namespace Tests
{
    [TestClass]
    public sealed class SocketFactoryTests
    {
        [TestMethod]
        public void Create_ShouldCreateNewSocket()
        {
            using (var factory = new SocketFactory(Mock.Of<IMessageReceiver>(), Mock.Of<IMessageSender>()))
            {
                var socket = factory.Create(PhysicalAddress.None, IPAddress.None);

                Assert.IsNotNull(socket);
            }
        }
    }
}
