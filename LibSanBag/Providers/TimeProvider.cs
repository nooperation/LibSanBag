using System;

namespace LibSanBag.Providers
{
    /// <inheritdoc />
    /// <summary>
    /// Provides a time source
    /// </summary>
    public class TimeProvider : ITimeProvider
    {
        /// <inheritdoc />
        /// <summary>
        /// Gets the current time in a Windows file time format
        /// </summary>
        /// <returns>Current time in a Windows file time format</returns>
        public ulong GetCurrentTime()
        {
            return (ulong)DateTime.Now.ToFileTime();
        }
    }
}
