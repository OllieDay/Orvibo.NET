using Microsoft.VisualStudio.TestTools.UnitTesting;
using Orvibo;

namespace Tests
{
    [TestClass]
    public sealed class SocketStateChangedEventArgsTests
    {
        [TestMethod]
        public void CreateNewSocketStateChangedEventArgs_FromState_ShouldBeValid()
        {
            var e = new SocketStateChangedEventArgs(SocketState.Off, SocketState.On);

            Assert.AreEqual(e.FromState, SocketState.Off);
        }

        [TestMethod]
        public void CreateNewSocketStateChangedEventArgs_ToState_ShouldBeValid()
        {
            var e = new SocketStateChangedEventArgs(SocketState.Off, SocketState.On);

            Assert.AreEqual(e.ToState, SocketState.On);
        }
    }
}
