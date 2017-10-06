using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSanBag.Tests
{
    [TestFixture(Category = nameof(FileRecordInfo))]
    public class TestFileRecordInfo
    {
        [Test]
        public void TestEmptyFilename()
        {
            var info = FileRecordInfo.Create("");
            Assert.AreEqual(info, null);
        }

        [Test]
        public void TestUnknownFilename()
        {
            var info = FileRecordInfo.Create("HelloWorld.txt");
            Assert.AreEqual(info, null);
        }

        [Test]
        public void TestImageFilename()
        {
            var info = FileRecordInfo.Create("0f74af948a58a66a96f4fc282a01ebf1.HelloWorld.png");
            Assert.IsTrue(info.IsRawImage);
        }

        [Test]
        public void TestResourceFilename()
        {
            var hash = "0f74af948a58a66a96f4fc282a01ebf1";
            var resourceType = "Texture-Resource";
            var versionHash = "9a8d4bbd19b4cd55";
            var contentType = "payload";
            var versionNumber = 0;
            var variants = "noVariants";

            var info = FileRecordInfo.Create($"{hash}.{resourceType}.v{versionHash}.{contentType}.v{versionNumber}.{variants}");
            Assert.NotNull(info);
            Assert.AreEqual(info.Hash, hash);
            Assert.AreEqual(info.ImagePath, null);
            Assert.AreEqual(info.IsRawImage, false);
            Assert.AreEqual(info.Type, FileRecordInfo.ResourceType.TextureResource);
            Assert.AreEqual(info.Variants, variants);
            Assert.AreEqual(info.VersionHash, versionHash);
            Assert.AreEqual(info.VersionNumber, versionNumber);
        }

        [Test]
        public void TestKnownResourceTypes()
        {
            var resourceNames = new Dictionary<string, FileRecordInfo.ResourceType>
            {
                ["Bank-Resource"] = FileRecordInfo.ResourceType.BankResource,
                ["Blueprint-Resource"] = FileRecordInfo.ResourceType.BlueprintResource,
                ["Cluster-Definition"] = FileRecordInfo.ResourceType.ClusterDefinition,
                ["Environment-Resource"] = FileRecordInfo.ResourceType.EnvironmentResource,
                ["GeometryResource-Resource"] = FileRecordInfo.ResourceType.GeometryResourceResource,
                ["hkaAnimationBinding-Resource"] = FileRecordInfo.ResourceType.AnimationBindingResource,
                ["hknpShape-Resource"] = FileRecordInfo.ResourceType.ShapeResource,
                ["ModelMorph-Resource"] = FileRecordInfo.ResourceType.ModelMorphResource,
                ["Pick-Resource"] = FileRecordInfo.ResourceType.PickResource,
                ["Sound-Resource"] = FileRecordInfo.ResourceType.SoundResource,
                ["Texture-Resource"] = FileRecordInfo.ResourceType.TextureResource,
                ["Texture-Source"] = FileRecordInfo.ResourceType.TextureSource,
                ["WorldChunk-Definition"] = FileRecordInfo.ResourceType.WorldChunkDefinition,
                ["World-Source"] = FileRecordInfo.ResourceType.WorldSource
            };

            foreach (var item in resourceNames)
            {
                var filename = $"0f74af948a58a66a96f4fc282a01ebf1.{item.Key}.v9a8d4bbd19b4cd55.payload.v0.noVariants";
                var info = FileRecordInfo.Create(filename);
                Assert.AreEqual(info.Type, item.Value);
                Assert.IsFalse(info.IsRawImage);
            }
        }
    }
}
