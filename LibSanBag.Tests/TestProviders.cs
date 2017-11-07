using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LibSanBag.Providers;
using NUnit.Framework;

namespace LibSanBag.Tests
{
    [TestFixture]
    class TestProviders
    {
        [Test]
        public void TestTimeProvider()
        {
            var provider = new TimeProvider();
            var previousTime = provider.GetCurrentTime();
            Thread.Sleep(1);
            var currentTime = provider.GetCurrentTime();

            Assert.GreaterOrEqual(currentTime, previousTime);
        }
    }
}
