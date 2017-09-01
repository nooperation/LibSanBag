using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSanBag
{
    public class TimeProvider : ITimeProvider
    {
        public ulong GetCurrentTime()
        {
            return (ulong)DateTime.Now.ToFileTime();
        }
    }
}
