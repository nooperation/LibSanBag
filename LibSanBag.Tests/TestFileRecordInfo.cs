using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibSanBag.Providers;
using static LibSanBag.FileRecordInfo;

namespace LibSanBag.Tests
{
    [TestFixture(Category = nameof(FileRecordInfo))]
    public class TestFileRecordInfo
    {
        readonly Dictionary<string, FileRecordInfo.ResourceType> ResourceNames = new Dictionary<string, FileRecordInfo.ResourceType>
        {
            ["Bank-Resource"] = FileRecordInfo.ResourceType.BankResource,
            ["Blueprint-Resource"] = FileRecordInfo.ResourceType.BlueprintResource,
            ["Cluster-Definition"] = FileRecordInfo.ResourceType.ClusterDefinition,
            ["Environment-Resource"] = FileRecordInfo.ResourceType.EnvironmentResource,
            ["GeometryResource-Resource"] = FileRecordInfo.ResourceType.GeometryResourceResource,
            ["ModelMorph-Resource"] = FileRecordInfo.ResourceType.ModelMorphResource,
            ["Pick-Resource"] = FileRecordInfo.ResourceType.PickResource,
            ["Sound-Resource"] = FileRecordInfo.ResourceType.SoundResource,
            ["Texture-Resource"] = FileRecordInfo.ResourceType.TextureResource,
            ["Texture-Source"] = FileRecordInfo.ResourceType.TextureSource,
            ["WorldChunk-Definition"] = FileRecordInfo.ResourceType.WorldChunkDefinition,
            ["World-Source"] = FileRecordInfo.ResourceType.WorldSource,
            ["ScriptMetadata-Resource"] = FileRecordInfo.ResourceType.ScriptMetadataResource,
            ["ScriptCompiledBytecode-Resource"] = FileRecordInfo.ResourceType.ScriptCompiledBytecodeResource,
            ["ScriptSourceText-Resource"] = FileRecordInfo.ResourceType.ScriptSourceTextResource,
            ["Clothing-Source"] = FileRecordInfo.ResourceType.ClothingSource,
            ["Cluster-Source"] = FileRecordInfo.ResourceType.ClusterSource,
            ["WorldChunk-Source"] = FileRecordInfo.ResourceType.WorldChunkSource,
            ["License-Resource"] = FileRecordInfo.ResourceType.LicenseResource,
            ["World-Definition"] = FileRecordInfo.ResourceType.WorldDefinition,
            ["AudioMaterial-Resource"] = FileRecordInfo.ResourceType.AudioMaterialResource,
            ["Material-Resource"] = FileRecordInfo.ResourceType.MaterialResource,
            ["Script-Resource"] = FileRecordInfo.ResourceType.ScriptResource,
            ["hkaAnimationBinding-Resource"] = FileRecordInfo.ResourceType.hkaAnimationBindingResource,
            ["hkaSkeleton-Resource"] = FileRecordInfo.ResourceType.hkaSkeletonResource,
            ["hknpMaterial-Resource"] = FileRecordInfo.ResourceType.hknpMaterialResource,
            ["hknpPhysicsSystemData-Resource"] = FileRecordInfo.ResourceType.hknpPhysicsSystemDataResource,
            ["hknpShape-Resource"] = FileRecordInfo.ResourceType.hknpShapeResource,
            ["Clothing-Import"] = FileRecordInfo.ResourceType.ClothingImport,
            ["File-Resource"] = FileRecordInfo.ResourceType.FileResource,
            ["Texture-Import"] = FileRecordInfo.ResourceType.TextureImport,
            ["GeometryResource-Canonical"] = FileRecordInfo.ResourceType.GeometryResourceCanonical,
            ["GeometryResource-Import"] = FileRecordInfo.ResourceType.GeometryResourceImport,
            ["Sound-Import"] = FileRecordInfo.ResourceType.SoundImport,
            ["hkbBehaviorGraph-Resource"] = FileRecordInfo.ResourceType.hkbBehaviorGraphResource,
            ["hkbCharacterData-Resource"] = FileRecordInfo.ResourceType.hkbCharacterDataResource,
            ["LuaScript-Resource"] = FileRecordInfo.ResourceType.LuaScriptResource,
            ["AudioGraph-Resource"] = FileRecordInfo.ResourceType.AudioGraphResource,
            ["Terrain-RuntimeTextureData"] = FileRecordInfo.ResourceType.TerrainRuntimeTextureData,
            ["Terrain-RuntimeDecalTextureData"] = FileRecordInfo.ResourceType.TerrainRuntimeDecalTextureData,
            ["BehaviorProject-Data"] = FileRecordInfo.ResourceType.BehaviorProjectData,
            ["hkbProjectData-Resource"] = FileRecordInfo.ResourceType.hkbProjectDataResource,
            ["PickableModel-Resource"] = FileRecordInfo.ResourceType.PickableModelResource,
            ["SpeechGraphicsEngine-Resource"] = FileRecordInfo.ResourceType.SpeechGraphicsEngineResource,
            ["SpeechGraphicsAnimation-Resource"] = FileRecordInfo.ResourceType.SpeechGraphicsAnimationResource,
            ["VertexDefinitionResource-Resource"] = FileRecordInfo.ResourceType.VertexDefinitionResourceResource,
            ["Buffer-Resource"] = FileRecordInfo.ResourceType.BufferResource,
            ["Mesh-Resource"] = FileRecordInfo.ResourceType.MeshResource,
            ["Terrain-RuntimeData"] = FileRecordInfo.ResourceType.TerrainRuntimeData,
            ["Terrain-SourceData"] = FileRecordInfo.ResourceType.TerrainSourceData,
            ["hkpConstraintData-Resource"] = FileRecordInfo.ResourceType.hkpConstraintDataResource,
            ["hkaSkeletonMapper-Resource"] = FileRecordInfo.ResourceType.hkaSkeletonMapperResource,
            ["hknpRagdollData-Resource"] = FileRecordInfo.ResourceType.hknpRagdollDataResource,
            ["Test-Resource"] = FileRecordInfo.ResourceType.TestResource,
            ["Script-Import"] = FileRecordInfo.ResourceType.ScriptImport,
            ["Heightmap-Import"] = FileRecordInfo.ResourceType.HeightmapImport,
            ["hknpShape-Import"] = FileRecordInfo.ResourceType.hknpShapeImport,
            ["Animation-Canonical"] = FileRecordInfo.ResourceType.AnimationCanonical,
            ["Animation-Import"] = FileRecordInfo.ResourceType.AnimationImport,
            ["Unknown"] = FileRecordInfo.ResourceType.Unknown
        };

        readonly Dictionary<string, FileRecordInfo.PayloadType> PayloadNames = new Dictionary<string, FileRecordInfo.PayloadType>
        {
            ["payload"] = PayloadType.Payload,
            ["manifest"] = PayloadType.Manifest,
            ["debug"] = PayloadType.Debug,
            ["capabilities"] = PayloadType.Capabilities,
            ["null"] = PayloadType.Null,
            ["unknown"] = PayloadType.Unknown,
        };

        readonly Dictionary<string, FileRecordInfo.VariantType> VariantNames = new Dictionary<string, FileRecordInfo.VariantType>
        {
            ["noVariants"] = VariantType.NoVariants,
            ["server"] = VariantType.Server,
            ["pcClient"] = VariantType.PcClient,
            ["unknown"] = VariantType.Unknown,
        };

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
            var payloadType = "payload";
            var versionNumber = 0;
            var variants = "noVariants";

            var info = FileRecordInfo.Create($"{hash}.{resourceType}.v{versionHash}.{payloadType}.v{versionNumber}.{variants}");
            Assert.NotNull(info);
            Assert.AreEqual(info.Hash, hash);
            Assert.AreEqual(info.ImagePath, null);
            Assert.AreEqual(info.IsRawImage, false);
            Assert.AreEqual(info.Resource, ResourceType.TextureResource);
            Assert.AreEqual(info.Variant, VariantType.NoVariants);
            Assert.AreEqual(info.Payload, PayloadType.Payload);
            Assert.AreEqual(info.VersionHash, versionHash);
            Assert.AreEqual(info.VersionNumber, versionNumber);
        }

        [Test]
        public void TestRecordCreationResourceNames()
        {
            foreach (var item in ResourceNames)
            {
                var filename = $"0f74af948a58a66a96f4fc282a01ebf1.{item.Key}.v9a8d4bbd19b4cd55.payload.v0.noVariants";
                var info = FileRecordInfo.Create(filename);
                Assert.AreEqual(info.Resource, item.Value);
                Assert.IsFalse(info.IsRawImage);
            }
        }

        [Test]
        public void TestRecordCreationVariantTypes()
        {
            var resourceNames = new Dictionary<string, VariantType>
            {
                ["noVariants"] = VariantType.NoVariants,
                ["server"] = VariantType.Server,
                ["pcClient"] = VariantType.PcClient,
                ["unknown"] = VariantType.Unknown
            };

            foreach (var item in resourceNames)
            {
                var filename = $"0f74af948a58a66a96f4fc282a01ebf1.Texture-Resource.v9a8d4bbd19b4cd55.payload.v0.{item.Key}";
                var info = FileRecordInfo.Create(filename);
                Assert.AreEqual(info.Variant, item.Value);
                Assert.IsFalse(info.IsRawImage);
            }
        }

        [Test]
        public void TestRecordCreationPayloadTypes()
        {
            var resourceNames = new Dictionary<string, PayloadType>
            {
                ["payload"] = PayloadType.Payload,
                ["manifest"] = PayloadType.Manifest,
                ["debug"] = PayloadType.Debug,
                ["capabilities"] = PayloadType.Capabilities,
                ["null"] = PayloadType.Null,
                ["unknown"] = PayloadType.Unknown,
            };

            foreach (var item in resourceNames)
            {
                var filename = $"0f74af948a58a66a96f4fc282a01ebf1.Texture-Resource.v9a8d4bbd19b4cd55.{item.Key}.v0.noVariants";
                var info = FileRecordInfo.Create(filename);
                Assert.AreEqual(info.Payload, item.Value);
                Assert.IsFalse(info.IsRawImage);
            }
        }

        [Test]
        public void TestKnownResourceTypes()
        {
            foreach (var item in ResourceNames)
            {
                var resourceType = FileRecordInfo.GetResourceType(item.Key);
                var expectedResourceType = item.Value;
                Assert.AreEqual(resourceType, expectedResourceType);
            }
        }

        [Test]
        public void TestKnownResourceTypeNames()
        {
            foreach (var item in ResourceNames)
            {
                var resourceTypeName = FileRecordInfo.GetResourceTypeName(item.Value);
                var expectedResourceTypeName = item.Key;
                Assert.AreEqual(resourceTypeName, expectedResourceTypeName);
            }
        }

        [Test]
        public void TestKnownPayloadTypes()
        {
            foreach (var item in PayloadNames)
            {
                var payloadType = FileRecordInfo.GetPayloadType(item.Key);
                var expectedPayloadType = item.Value;
                Assert.AreEqual(payloadType, expectedPayloadType);
            }
        }

        [Test]
        public void TestKnownPayloadTypeNames()
        {
            foreach (var item in PayloadNames)
            {
                var payloadTypeName = FileRecordInfo.GetPayloadTypeName(item.Value);
                var expectedPayloadTypeName = item.Key;
                Assert.AreEqual(payloadTypeName, expectedPayloadTypeName);
            }
        }

        [Test]
        public void TestKnownVariantTypes()
        {
            foreach (var item in VariantNames)
            {
                var variantType = FileRecordInfo.GetVariantType(item.Key);
                var expectedVariantType = item.Value;
                Assert.AreEqual(variantType, expectedVariantType);
            }
        }

        [Test]
        public void TestKnownVariantTypeNames()
        {
            foreach (var item in VariantNames)
            {
                var variantTypeName = FileRecordInfo.GetVariantTypeName(item.Value);
                var expectedVariantTypeName = item.Key;
                Assert.AreEqual(variantTypeName, expectedVariantTypeName);
            }
        }
    }
}
