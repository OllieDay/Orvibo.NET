using System;
using System.Runtime.Serialization;

namespace Orvibo.Messaging
{
    /// <summary>
    ///     Represents error that occur during message sending and receiving.
    /// </summary>
    [Serializable]
    public class MessageException : OrviboException
    {
        /// <summary>
        ///     Initializes a new instance of the MessageException class.
        /// </summary>
        internal MessageException() {}

        /// <summary>
        ///     Initializes a new instance of the MessageException class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        internal MessageException(string message) : base(message) {}

        /// <summary>
        ///     Initializes a new instance of the MessageException class with a specified error message and a reference to the
        ///     inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">
        ///     The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner
        ///     exception is specified.
        /// </param>
        internal MessageException(string message, Exception innerException) : base(message, innerException) {}

        /// <summary>
        ///     Initializes a new instance of the MessageException class with serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        internal MessageException(SerializationInfo info, StreamingContext context) : base(info, context) {}
    }
}
