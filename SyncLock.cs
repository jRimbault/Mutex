using System;
using System.Threading;

namespace Sync
{
    public sealed class SyncLock<T> where T : notnull
    {
        private readonly object _locker = new object();
        private readonly int _timeout = Timeout.Infinite;
        private readonly T _inner;

        public SyncLock(T value) : this(value, Timeout.Infinite) { }

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


        public ILockGuard<T> Lock()
        {
            if (!System.Threading.Monitor.TryEnter(_locker, _timeout))
            {
                throw new Exception($"Failed to acquire lock for {nameof(T)}");
            }
            return new LockGuard<T>(this);
        }

        private void Unlock()
        {
            Monitor.Exit(_locker);
        }

        public interface ILockGuard<U> : IDisposable where U : notnull
        {
            public U Value { get; }
        }

        private sealed class LockGuard<U> : ILockGuard<U> where U : notnull
        {
            public U Value { get => _parent._inner; }
            private readonly SyncLock<U> _parent;

            public LockGuard(SyncLock<U> parent) => _parent = parent;

            public void Dispose()
            {
                _parent.Unlock();
            }
        }
    }
}
