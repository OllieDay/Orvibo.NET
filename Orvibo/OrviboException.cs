using System;
using System.Runtime.Serialization;

namespace Orvibo
{
    /// <summary>
    ///     Represents Orvibo errors that occur during application execution.
    /// </summary>
    [Serializable]
    public class OrviboException : Exception
    {
        /// <summary>
        ///     Initializes a new instance of the OrviboException class.
        /// </summary>
        internal OrviboException() {}

        /// <summary>
        ///     Initializes a new instance of the OrviboException class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        internal OrviboException(string message) : base(message) {}

        /// <summary>
        ///     Initializes a new instance of the OrviboException class with a specified error message and a reference to the inner
        ///     exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">
        ///     The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner
        ///     exception is specified.
        /// </param>
        internal OrviboException(string message, Exception innerException) : base(message, innerException) {}

        /// <summary>
        ///     Initializes a new instance of the OrviboException class with serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        internal OrviboException(SerializationInfo info, StreamingContext context) : base(info, context) {}
    }
}
