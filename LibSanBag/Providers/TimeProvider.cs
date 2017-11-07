using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibSanBag.Providers;

namespace LibSanBag.Providers
{
    /// <summary>
    /// Provides a time source
    /// </summary>
    public class TimeProvider : ITimeProvider
    {
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
