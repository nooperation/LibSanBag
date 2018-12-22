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
        public void TestGetResourceVersions()
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

        [Test]
        public void TestGetResourceTypeFromVersion()
        {
            var enums = Enum.GetValues(typeof(FileRecordInfo.ResourceType));
            foreach (var item in enums)
            {
                var expectedResourceType = (FileRecordInfo.ResourceType)item;
                var versions = AssetVersions.GetResourceVersions(expectedResourceType);
                if (versions.Count > 0)
                {
                    for(int i = 0; i < versions.Count; ++i) {
                        // TODO: Fix GetResourceVersions now that assets are no longer have a version hash
                        // For now just check version hash versions and not numbered versions.
                        if(versions[i].Length == 16) {
                            var resourceType = AssetVersions.GetResourceTypeFromVersion(versions[i]);
                            Assert.AreEqual(expectedResourceType, resourceType);
                        }
                    }
                }
            }
        }

        [Test]
        public void TestGetResourceTypeFromVersionUnknown()
        {
            var resourceType = AssetVersions.GetResourceTypeFromVersion("UnknownVersion");
            Assert.AreEqual(FileRecordInfo.ResourceType.Unknown, resourceType);
        }
    }
}
