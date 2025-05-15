using System;

namespace Qase.Csharp.Commons.Core
{
    /// <summary>
    /// Exception thrown by Qase related operations
    /// </summary>
    public class QaseException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the QaseException class
        /// </summary>
        public QaseException() : base() { }

        /// <summary>
        /// Initializes a new instance of the QaseException class with a specified error message
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        public QaseException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the QaseException class with a specified error message
        /// and a reference to the inner exception that is the cause of this exception
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        /// <param name="innerException">The exception that is the cause of the current exception</param>
        public QaseException(string message, Exception innerException) : base(message, innerException) { }
    }
} 
