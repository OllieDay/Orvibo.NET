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
            using (var messageReceiver = new MessageReceiver(Mock.Of<IMessageParser>()))
            {
                Assert.AreEqual(messageReceiver.Blacklist.Count, 0);
            }
        }

        [TestMethod]
        public void CreateNewMessageReceiver_Blacklist_ShouldBeValid()
        {
            var blacklist = new List<IPAddress>
            {
                IPAddress.None
            };

            using (var messageReceiver = new MessageReceiver(Mock.Of<IMessageParser>(), blacklist))
            {
                Assert.AreEqual(messageReceiver.Blacklist, blacklist);
            }
        }

        [TestMethod]
        public void CreateNewMessageReceiver_IsRunning_ShouldBeFalse()
        {
            using (var messageReceiver = new MessageReceiver(Mock.Of<IMessageParser>()))
            {
                Assert.IsFalse(messageReceiver.IsRunning);
            }
        }

        [TestMethod]
        public void MessageReceived_MessageReceived_ShouldBeInvoked()
        {
            var message = Mock.Of<IInboundMessage>();
            var messageParser = Mock.Of<IMessageParser>(mp => mp.Parse(It.IsAny<byte[]>()) == message);

            using (var messageReceiver = new MessageReceiver(messageParser))
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
            var message = Mock.Of<IInboundMessage>();
            var messageParser = Mock.Of<IMessageParser>(mp => mp.Parse(It.IsAny<byte[]>()) == message);

            var blacklist = new List<IPAddress>
            {
                IPAddress.Loopback
            };

            using (var messageReceiver = new MessageReceiver(messageParser, blacklist))
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
            using (var messageReceiver = new MessageReceiver(Mock.Of<IMessageParser>()))
            {
                messageReceiver.Start();
                messageReceiver.Start();
            }
        }

        [TestMethod]
        public void Start_IsRunning_ShouldBeTrue()
        {
            using (var messageReceiver = new MessageReceiver(Mock.Of<IMessageParser>()))
            {
                messageReceiver.Start();

                Assert.IsTrue(messageReceiver.IsRunning);
            }
        }

        [TestMethod]
        public void Stop_IsRunning_ShouldBeFalse()
        {
            using (var messageReceiver = new MessageReceiver(Mock.Of<IMessageParser>()))
            {
                messageReceiver.Start();
                messageReceiver.Stop();

                Assert.IsFalse(messageReceiver.IsRunning);
            }
        }
    }
}
