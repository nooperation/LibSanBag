using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibSanBag.Providers;
using LibSanBag.ResourceUtils;
using NUnit.Framework;

namespace LibSanBag.Tests
{
    class MockRegistryProvider : IRegistryProvider
    {
        public Queue<object> ReturnValueQueue { get; set; } = new Queue<object>();

        public object GetValue(string keyName, string valueName, object defaultValue) => ReturnValueQueue.Dequeue();
    }

    [TestFixture]
    class TestUtils
    {
        [Test]
        public void TestGetSansarDirectory()
        {
            var expectedValue = @"C:\Program Files\Sansar\";
            var registryProvider = new MockRegistryProvider();
            registryProvider.ReturnValueQueue.Enqueue(expectedValue);

            var result = Utils.GetSansarDirectory(registryProvider);
            Assert.AreEqual(result, expectedValue);
        }

        [Test]
        public void TestGetSansarDirectoryFallback()
        {
            var expectedValue = @"C:\Program Files\Sansar";
            var registryProvider = new MockRegistryProvider();
            registryProvider.ReturnValueQueue.Enqueue(null);
            registryProvider.ReturnValueQueue.Enqueue(@"C:\Program Files\Sansar\Updater\Sansar.exe,1");

            var result = Utils.GetSansarDirectory(registryProvider);
            Assert.AreEqual(result, expectedValue);
        }
    }
}
