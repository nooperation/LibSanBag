using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSanBag
{
    public interface ITimeProvider
    {
        ulong GetCurrentTime();
    }
}
