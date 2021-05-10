using System;

namespace Sync
{
    /// <summary>
    /// Exception thrown when a Lock method on the Mutex class times out.
    /// </summary>
    public class MutexException : Exception
    {
        /// <summary>
        /// Constructs an instance with the specified message.
        /// </summary>
        /// <param name="message">The message for the exception</param>
        internal MutexException(string message) : base(message)
        {
        }
    }
}
