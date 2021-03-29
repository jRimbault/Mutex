using System;

namespace Sync
{
    /// <summary>
    /// A guard which provides exclusive access to the inner <c>T</c>,
    /// that guard must be disposed of to release the internal monitor (i.e. to unlock).
    /// </summary>
    public sealed class MutexGuard<T> : IDisposable
        where T : notnull
    {
        public T Value { get => _parent._inner; }
        private bool _isDisposed = false;
        private readonly Mutex<T> _parent;

        internal MutexGuard(Mutex<T> parent) => _parent = parent;

        /// <summary>
        /// Releases the lock. Subsequent calls to this method do nothing.
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }
            _parent.Unlock();
            _isDisposed = true;
        }
    }
}
