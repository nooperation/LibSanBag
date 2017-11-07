using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibSanBag.Providers;

namespace LibSanBag.Tests.Providers
{
    class MockRegistryProvider : IRegistryProvider
    {
        public Queue<object> ReturnValueQueue { get; } = new Queue<object>();

        public object GetValue(string keyName, string valueName, object defaultValue) => ReturnValueQueue.Dequeue();
    }
}
