using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Orvibo.Messaging.Inbound;

namespace Tests
{
    [TestClass]
    public sealed class MessageReceiverTests
    {
        [TestMethod]
        public void CreateNewMessageReceiver_Blacklist_ShouldBeEmpty()
        {
            using (var messageReceiver = CreateMessageReceiver())
            {
                Assert.AreEqual(0, messageReceiver.Blacklist.Count);
            }
        }

        [TestMethod]
        public void CreateNewMessageReceiver_Blacklist_ShouldBeValid()
        {
            var blacklist = new List<IPAddress>();

            using (var messageReceiver = CreateMessageReceiver(blacklist))
            {
                Assert.AreEqual(messageReceiver.Blacklist, blacklist);
            }
        }

        [TestMethod]
        public void CreateNewMessageReceiver_IsRunning_ShouldBeFalse()
        {
            using (var messageReceiver = CreateMessageReceiver())
            {
                Assert.IsFalse(messageReceiver.IsRunning);
            }
        }

        [TestMethod]
        public void MessageReceived_MessageReceived_ShouldBeInvoked()
        {
            using (var messageReceiver = CreateMessageReceiver())
            {
                var invoked = false;

                messageReceiver.MessageReceived += (sender, e) => invoked = true;
                messageReceiver.Start();

                using (var client = new UdpClient())
                {
                    client.Send(new byte[0], 0, new IPEndPoint(IPAddress.Loopback, 10000));
                }

                Thread.Sleep(10);
                Assert.IsTrue(invoked);
            }
        }

        [TestMethod]
        public void MessageReceivedFromBlacklistedIPAddress_MessageReceived_ShouldNotBeInvoked()
        {
            var blacklist = new List<IPAddress>
            {
                IPAddress.Loopback
            };

            using (var messageReceiver = CreateMessageReceiver(blacklist))
            {
                var invoked = false;

                messageReceiver.MessageReceived += (sender, e) => invoked = true;
                messageReceiver.Start();

                using (var client = new UdpClient())
                {
                    client.Send(new byte[0], 0, new IPEndPoint(IPAddress.Loopback, 10000));
                }

                Thread.Sleep(10);
                Assert.IsFalse(invoked);
            }
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void Start_AlreadyRunning_ShouldThrowInvalidOperationException()
        {
            using (var messageReceiver = CreateMessageReceiver())
            {
                messageReceiver.Start();
                messageReceiver.Start();
            }
        }

        [TestMethod]
        public void Start_IsRunning_ShouldBeTrue()
        {
            using (var messageReceiver = CreateMessageReceiver())
            {
                messageReceiver.Start();

                Assert.IsTrue(messageReceiver.IsRunning);
            }
        }

        [TestMethod]
        public void Stop_IsRunning_ShouldBeFalse()
        {
            using (var messageReceiver = CreateMessageReceiver())
            {
                messageReceiver.Start();
                messageReceiver.Stop();

                Assert.IsFalse(messageReceiver.IsRunning);
            }
        }

        private IMessageParser CreateMessageParser()
        {
            var message = Mock.Of<IInboundMessage>();
            var messageParser = Mock.Of<IMessageParser>(mp => mp.Parse(It.IsAny<byte[]>()) == message);

            return messageParser;
        }

        private IMessageReceiver CreateMessageReceiver(List<IPAddress> blacklist)
        {
            return new MessageReceiver(CreateMessageParser(), blacklist);
        }

        private IMessageReceiver CreateMessageReceiver()
        {
            return new MessageReceiver(CreateMessageParser(), new List<IPAddress>());
        }
    }
}
