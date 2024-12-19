using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordToPdfExporter.Core.Lock;

public interface ILockService
{
    /// <summary>
    /// Acquires a lock. Waits if the lock is already held by another process.
    /// </summary>
    /// <param name="timeout">The maximum time to wait for the lock.</param>
    /// <returns>True if the lock was acquired; otherwise, false.</returns>
    Task<bool> LockAsync(TimeSpan timeout);

    /// <summary>
    /// Checks if the resource is locked and waits for it to be unlocked.
    /// </summary>
    /// <param name="timeout">The maximum time to wait for the resource to be unlocked.</param>
    /// <returns>True if the resource was unlocked within the timeout; otherwise, false.</returns>
    Task<bool> WaitForUnlockAsync(TimeSpan timeout);

    /// <summary>
    /// Releases the lock.
    /// </summary>
    void Unlock();
}
