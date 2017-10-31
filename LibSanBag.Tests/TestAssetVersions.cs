using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace LibSanBag.Tests
{
    [TestFixture]
    class TestAssetVersions
    {
        [Test]
        public void TestCreateEmptyBag()
        {
            var enums = Enum.GetValues(typeof(FileRecordInfo.ResourceType));
            foreach (var item in enums)
            {
                Assert.DoesNotThrow(() =>
                {
                    AssetVersions.GetResourceVersions((FileRecordInfo.ResourceType) item);
                });
            }
        }
    }
}
