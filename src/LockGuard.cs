using System;

namespace Sync
{
    /// <summary>
    /// A guard which provides exclusive access to the inner <c>T</c>,
    /// that guard must be disposed of to release the internal monitor (i.e. to unlock).
    /// </summary>
    public interface ILockGuard<U> : IDisposable where U : notnull
    {
        public U Value { get; }
    }
}
