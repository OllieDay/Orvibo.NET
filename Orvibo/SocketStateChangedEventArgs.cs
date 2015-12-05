using System;

namespace Orvibo
{
    /// <summary>
    ///     Event args for the socket state changed event.
    /// </summary>
    public sealed class SocketStateChangedEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the SocketStateChangedEventArgs class.
        /// </summary>
        /// <param name="fromState">State before the state change occured.</param>
        /// <param name="toState">State after the state change occured.</param>
        internal SocketStateChangedEventArgs(SocketState fromState, SocketState toState)
        {
            FromState = fromState;
            ToState = toState;
        }

        /// <summary>
        ///     State before the state change occured.
        /// </summary>
        public SocketState FromState { get; }

        /// <summary>
        ///     State after the state change occured.
        /// </summary>
        public SocketState ToState { get; }
    }
}
