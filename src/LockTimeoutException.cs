using System;

namespace Sync
{
    /// <summary>
    /// Exception thrown when a Lock method on the SyncLock class times out.
    /// </summary>
    public class LockTimeoutException : Exception
    {
        /// <summary>
        /// Constructs an instance with the specified message.
        /// </summary>
        /// <param name="message">The message for the exception</param>
        internal LockTimeoutException(string message) : base(message)
        {
        }
    }
}
