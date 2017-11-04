using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSanBag.Providers
{
    class EnvironmentProvider : IEnvironmentProvider
    {
        public string GetEnvironmentVariable(string variable, EnvironmentVariableTarget target)
        {
            return Environment.GetEnvironmentVariable(variable, target);
        }

        public void SetEnvironmentVariable(string variable, string value, EnvironmentVariableTarget target)
        {
            Environment.SetEnvironmentVariable(variable, value, target);
        }
    }
}
