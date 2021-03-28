using System.Threading;

namespace Sync
{
    /// <summary>
    /// Class used for locking, as an alternative to just locking on normal monitors.
    /// The Lock method returns a guard which provides exclusive access to the inner 
    /// <c>T</c>, that guard must then be disposed of to release the internal monitor
    /// (i.e. to unlock).
    /// </summary>
    public sealed class Mutex<T> where T : notnull
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
        /// A lock guard with exclusive access to <c>T</c> which then should be disposed of to release the lock
        /// </returns>
        /// <exception cref="LockTimeoutException">The operation times out.</exception>
        public MutexGuard<T> Lock()
        {
            if (!Monitor.TryEnter(_monitor))
            {
                throw new LockTimeoutException($"Failed to acquire lock for {nameof(T)}");
            }
            return new MutexGuard<T>(this);
        }

        internal void Unlock()
        {
            Monitor.Exit(_monitor);
        }
    }
}
