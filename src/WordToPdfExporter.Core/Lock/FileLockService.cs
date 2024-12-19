using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordToPdfExporter.Core.Lock;
public class FileLockService : ILockService
{
    private readonly string _lockFilePath;

    public FileLockService(string lockFilePath)
    {
        _lockFilePath = lockFilePath;
    }

    public async Task<bool> LockAsync(TimeSpan timeout)
    {
        var startTime = DateTime.UtcNow;

        while (true)
        {
            // Check if the lock file exists
            if (File.Exists(_lockFilePath))
            {
                // Check if the timeout has been exceeded
                if (DateTime.UtcNow - startTime > timeout)
                {
                    return false; // Could not acquire the lock within the timeout
                }

                // Wait for a short period before retrying
                await Task.Delay(100);
            }
            else
            {
                try
                {
                    // Create the lock file to indicate the lock is acquired
                    await File.Create(_lockFilePath).DisposeAsync();
                    return true; // Lock acquired successfully
                }
                catch
                {
                    // If another process created the file at the same time, retry
                    await Task.Delay(100);
                }
            }
        }
    }

    public async Task<bool> WaitForUnlockAsync(TimeSpan timeout)
    {
        DateTime startTime = DateTime.UtcNow;

        while (File.Exists(_lockFilePath))
        {
            // Check if the timeout has been exceeded
            if (DateTime.UtcNow - startTime > timeout)
            {
                return false; // Lock was not released within the timeout
            }

            // Wait for a short period before checking again
            await Task.Delay(100);
        }

        return true; // Lock was released
    }

    public void Unlock()
    {
        try
        {
            // Delete the lock file to release the lock
            if (File.Exists(_lockFilePath))
            {
                File.Delete(_lockFilePath);
            }
        }
        catch (Exception ex)
        {
            // Handle potential exceptions, e.g., permission issues
            Console.WriteLine($"Failed to unlock: {ex.Message}");
        }
    }
}
