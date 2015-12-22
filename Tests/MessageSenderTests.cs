using System;
using System.Net;
using System.Net.Sockets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Orvibo.Messaging.Outbound;

namespace Tests
{
    [TestClass]
    public sealed class MessageSenderTests
    {
        private const int Port = 10000;

        [TestMethod]
        public void Send_ReceivedData_ShouldMatch()
        {
            var message = Mock.Of<IOutboundMessage>(m => m.GetPayload() == new byte[] { 1, 2, 3 });

            using (var client = new UdpClient(Port))
            {
                var result = client.ReceiveAsync();

                using (var messageSender = new MessageSender())
                {
                    messageSender.Send(message, IPAddress.Loopback);
                }

                result.Wait(TimeSpan.FromSeconds(1));

                var expected = new byte[] { 1, 2, 3 };
                var actual = result.Result.Buffer;

                CollectionAssert.AreEqual(expected, actual);
            }
        }
    }
}
