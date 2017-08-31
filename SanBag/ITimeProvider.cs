using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanBag
{
    public interface ITimeProvider
    {
        ulong GetCurrentTime();
    }
}
