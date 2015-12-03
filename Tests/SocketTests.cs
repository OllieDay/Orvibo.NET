using System;
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
    public sealed class SocketTests
    {
        [TestMethod]
        public void CreateNewSocket_IsSubscribed_ShouldBeFalse()
        {
            var socket = new Socket(PhysicalAddress.None, IPAddress.None, null);

            Assert.IsFalse(socket.IsSubscribed);
        }

        [TestMethod]
        public void CreateNewSocket_State_ShouldBeUnknown()
        {
            var socket = new Socket(PhysicalAddress.None, IPAddress.None, null);

            Assert.AreEqual(socket.State, SocketState.Unknown);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void Off_IsSubscribedIsFalse_ShouldThrowInvalidOperationException()
        {
            var socket = new Socket(PhysicalAddress.None, IPAddress.None, null);

            socket.Off();
        }

        [TestMethod]
        public void Off_StateIsNotOff_ShouldSendOffMessage()
        {
            var unicastMessageSender = Mock.Of<IUnicastMessageSender>();
            var socket = new Socket(PhysicalAddress.None, IPAddress.None, unicastMessageSender);
            var data = new byte[] { 0x68, 0x64, 0x00, 0x18, 0x63, 0x6C, 0xAC, 0xCF, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 };
            var message = new InboundSubscribeMessage(data);

            socket.Process(message);
            socket.Off();

            Mock.Get(unicastMessageSender).Verify(ums => ums.SendOffMessage(It.IsAny<INetworkDevice>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void Off_StateIsOff_ShouldThrowInvalidOperationException()
        {
            var socket = new Socket(PhysicalAddress.None, IPAddress.None, null);
            var data = new byte[] { 0x68, 0x64, 0x00, 0x18, 0x63, 0x6C, 0xAC, 0xCF, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            var message = new InboundSubscribeMessage(data);

            socket.Process(message);

            socket.Off();
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void On_IsSubscribedIsFalse_ShouldThrowInvalidOperationException()
        {
            var socket = new Socket(PhysicalAddress.None, IPAddress.None, null);

            socket.Off();
        }

        [TestMethod]
        public void On_StateIsNotOn_ShouldSendOnMessage()
        {
            var unicastMessageSender = Mock.Of<IUnicastMessageSender>();
            var socket = new Socket(PhysicalAddress.None, IPAddress.None, unicastMessageSender);
            var data = new byte[] { 0x68, 0x64, 0x00, 0x18, 0x63, 0x6C, 0xAC, 0xCF, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            var message = new InboundSubscribeMessage(data);

            socket.Process(message);
            socket.On();

            Mock.Get(unicastMessageSender).Verify(ums => ums.SendOnMessage(It.IsAny<INetworkDevice>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void On_StateIsOn_ShouldThrowInvalidOperationException()
        {
            var socket = new Socket(PhysicalAddress.None, IPAddress.None, null);
            var data = new byte[] { 0x68, 0x64, 0x00, 0x18, 0x63, 0x6C, 0xAC, 0xCF, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 };
            var message = new InboundSubscribeMessage(data);

            socket.Process(message);

            socket.On();
        }

        [TestMethod]
        public void StateChanged_State_ShouldChange()
        {
            var socket = new Socket(PhysicalAddress.None, IPAddress.None, null);
            var data = new byte[] { 0x68, 0x64, 0x00, 0x18, 0x63, 0x6C, 0xAC, 0xCF, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            var message = new InboundSubscribeMessage(data);

            socket.Process(message);

            Assert.AreEqual(socket.State, SocketState.Off);
        }

        [TestMethod]
        public void Subscribe_IsSubscribedIsFalse_ShouldSendSubscribeMessage()
        {
            var unicastMessageSender = Mock.Of<IUnicastMessageSender>();
            var socket = new Socket(PhysicalAddress.None, IPAddress.None, unicastMessageSender);

            socket.Subscribe();

            Mock.Get(unicastMessageSender).Verify(ums => ums.SendSubscribeMessage(It.IsAny<INetworkDevice>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void Subscribe_IsSubscribeIsTrue_ShouldThrowInvalidOperationException()
        {
            var socket = new Socket(PhysicalAddress.None, IPAddress.None, null);
            var data = new byte[] { 0x68, 0x64, 0x00, 0x18, 0x63, 0x6C, 0xAC, 0xCF, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            var message = new InboundSubscribeMessage(data);

            socket.Process(message);

            socket.Subscribe();
        }

        [TestMethod]
        public void Subscribe_Subscribed_ShouldBeRaised()
        {
            var socket = new Socket(PhysicalAddress.None, IPAddress.None, null);
            var data = new byte[] { 0x68, 0x64, 0x00, 0x18, 0x63, 0x6C, 0xAC, 0xCF, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            var message = new InboundSubscribeMessage(data);
            var invoked = false;

            socket.Subscribed += (sender, e) => invoked = true;
            socket.Process(message);

            Assert.IsTrue(invoked);
        }

        [TestMethod]
        public void Unsubscribe_IsSubscribed_ShouldBeFalse()
        {
            var socket = new Socket(PhysicalAddress.None, IPAddress.None, null);
            var data = new byte[] { 0x68, 0x64, 0x00, 0x18, 0x63, 0x6C, 0xAC, 0xCF, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            var message = new InboundSubscribeMessage(data);

            socket.Process(message);
            socket.Unsubscribe();

            Assert.IsFalse(socket.IsSubscribed);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void Unsubscribe_IsSubscribeIsFalse_ShouldThrowInvalidOperationException()
        {
            var socket = new Socket(PhysicalAddress.None, IPAddress.None, null);

            socket.Unsubscribe();
        }

        [TestMethod]
        public void Unsubscribe_Unsubscribed_ShouldBeRaised()
        {
            var socket = new Socket(PhysicalAddress.None, IPAddress.None, null);
            var data = new byte[] { 0x68, 0x64, 0x00, 0x18, 0x63, 0x6C, 0xAC, 0xCF, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            var message = new InboundSubscribeMessage(data);

            socket.Process(message);

            var invoked = false;

            socket.Unsubscribed += (sender, e) => invoked = true;
            socket.Unsubscribe();

            Assert.IsTrue(invoked);
        }
    }
}
