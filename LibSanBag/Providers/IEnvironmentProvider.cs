using System;
using System.Security;

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
