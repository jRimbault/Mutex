using System;
using System.Threading;

namespace Sync
{
    /// <summary>
    /// Class used for locking, as an alternative to just locking on normal monitors.
    /// The <c>Lock</c> method returns a guard which provides exclusive access to the inner
    /// <c>T</c>, that guard must then be disposed of to release the internal monitor
    /// (i.e. to unlock).
    /// </summary>
    public sealed class Mutex<T>
        where T : notnull
    {
        private readonly object _monitor = new object();
        internal readonly T _inner;

        public Mutex(T value)
        {
            _inner = value;
        }

        /// <summary>
        /// Locks the monitor.
        /// </summary>
        /// <returns>
        /// A lock guard with exclusive access to <c>T</c> which then should be disposed of to release the lock.
        /// </returns>
        /// <exception cref="MutexException">The operation times out.</exception>
        public MutexGuard<T> Lock()
        {
            return Lock(Timeout.Infinite);
        }

        /// <summary>
        /// Locks the monitor.
        /// </summary>
        /// <returns>
        /// A lock guard with exclusive access to <c>T</c> which then should be disposed of to release the lock.
        /// </returns>
        /// <exception cref="MutexException">The operation times out.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The timeout specified is invalid.</exception>
        public MutexGuard<T> Lock(TimeSpan timeout)
        {
            long millis = (long)timeout.TotalMilliseconds;
            if (millis > int.MaxValue)
            {
                throw new ArgumentOutOfRangeException("Invalid timeout specified");
            }
            return Lock((int)millis);
        }

        /// <summary>
        /// Locks the monitor.
        /// </summary>
        /// <returns>
        /// A lock guard with exclusive access to <c>T</c> which then should be disposed of to release the lock.
        /// </returns>
        /// <exception cref="MutexException">The operation times out.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The timeout specified is invalid.</exception>
        public MutexGuard<T> Lock(int timeout)
        {
            if (timeout < Timeout.Infinite)
            {
                throw new ArgumentOutOfRangeException("Invalid timeout specified");
            }
            if (!Monitor.TryEnter(_monitor, timeout))
            {
                throw new MutexException($"Failed to acquire lock for {typeof(T)}");
            }
            return new(this);
        }

        internal void Unlock()
        {
            Monitor.Exit(_monitor);
        }
    }
}
