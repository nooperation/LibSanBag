using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSanBag.Providers
{
    public interface IRegistryProvider
    {
        object GetValue(string keyName, string valueName, object defaultValue);
    }
}
