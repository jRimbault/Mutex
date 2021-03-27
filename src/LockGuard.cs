using System;

namespace Sync
{
    /// <summary>
    /// A guard which provides exclusive access to the inner <c>T</c>,
    /// that guard must be disposed of to release the internal monitor (i.e. to unlock).
    /// </summary>
    public sealed class LockGuard<U> : IDisposable where U : notnull
    {
        public U Value { get => _parent._inner; }
        private bool _isDisposed = false;
        private readonly SyncLock<U> _parent;

        internal LockGuard(SyncLock<U> parent) => _parent = parent;

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
