using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace LibSanBag.Providers
{
    public interface IEnvironmentProvider
    {
        [SecuritySafeCritical]
        string GetEnvironmentVariable(string variable, EnvironmentVariableTarget target);

        [SecuritySafeCritical]
        void SetEnvironmentVariable(string variable, string value, EnvironmentVariableTarget target);
    }
}
