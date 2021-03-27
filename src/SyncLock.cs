﻿using System;
using System.Threading;

namespace Sync
{
    /// <summary>
    /// Class used for locking, as an alternative to just locking on normal monitors.
    /// Allows for timeouts when locking, and each Lock method returns a guard which
    /// provides exclusive access to the inner <c>T</c>, that guard must then be disposed
    /// of to release the internal monitor (i.e. to unlock).
    /// </summary>
    public sealed class SyncLock<T> where T : notnull
    {
        private readonly object _monitor = new object();
        private readonly int _timeout = Timeout.Infinite;
        internal readonly T _inner;

        public SyncLock(T value) : this(value, Timeout.Infinite)
        {
        }

        public SyncLock(T value, int timeout)
        {
            if (timeout < Timeout.Infinite)
            {
                throw new ArgumentOutOfRangeException("Invalid timeout specified");
            }
            _inner = value;
            _timeout = timeout;
        }

        public SyncLock(T value, TimeSpan timeout)
        {
            long millis = (long)timeout.TotalMilliseconds;
            if (millis < Timeout.Infinite || millis > int.MaxValue)
            {
                throw new ArgumentOutOfRangeException("Invalid timeout specified");
            }
            _inner = value;
            _timeout = (int)millis;
        }

        /// <summary>
        /// Locks the monitor.
        /// </summary>
        /// <returns>
        /// A lock guard with exclusive access to <c>T</c> which then should be disposed of to release the lock
        /// </returns>
        /// <exception cref="LockTimeoutException">The operation times out.</exception>
        public LockGuard<T> Lock()
        {
            if (!Monitor.TryEnter(_monitor, _timeout))
            {
                throw new LockTimeoutException($"Failed to acquire lock for {nameof(T)}");
            }
            return new LockGuard<T>(this);
        }

        internal void Unlock()
        {
            Monitor.Exit(_monitor);
        }
    }
}
