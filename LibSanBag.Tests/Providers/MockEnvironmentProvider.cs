using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibSanBag.Providers;

namespace LibSanBag.Tests.Providers
{
    internal class MockEnvironmentProvider : IEnvironmentProvider
    {
        public Queue<string> EnvironmentVariableQueue { get; } = new Queue<string>();
        public string GetEnvironmentVariable(string variable, EnvironmentVariableTarget target)
        {
            return EnvironmentVariableQueue.Dequeue();
        }

        public string LastSetVariable { get; private set; }
        public string LastSetValue { get; private set; }
        public EnvironmentVariableTarget LastSetTarget { get; private set; }

        public void SetEnvironmentVariable(string variable, string value, EnvironmentVariableTarget target)
        {
            LastSetVariable = variable;
            LastSetValue = value;
            LastSetTarget = target;
        }
    }
}
