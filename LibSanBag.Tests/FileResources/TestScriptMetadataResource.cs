using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibSanBag.FileResources;
using LibSanBag.FileResources.Legacy;
using Newtonsoft.Json;
using NUnit.Framework;

namespace LibSanBag.Tests.FileResources
{
    [TestFixture]
    class TestScriptMetadataResource : BaseFileResourceTest
    {
        private struct TestData
        {
            public string CompressedFilePath { get; }
            public string JsonFilePath { get; }
            public FileRecordInfo RecordInfo { get; }

            public TestData(string compressedFilePath, string jsonFilePath)
            {
                JsonFilePath = jsonFilePath;
                CompressedFilePath = Path.Combine(RootPath, compressedFilePath);
                RecordInfo = FileRecordInfo.Create(compressedFilePath);
            }
        }

        private static readonly string RootPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Resources", "ScriptMetadata");
        private static readonly List<TestData> Tests = new List<TestData>();

        [SetUp]
        public override void Setup()
        {
            base.Setup();

            var jsonFilePaths = Directory.GetFiles(RootPath, "*.json", SearchOption.AllDirectories);
            foreach (var jsonFilePath in jsonFilePaths)
            {
                var resourcePath = Path.Combine(Path.GetDirectoryName(jsonFilePath), Path.GetFileNameWithoutExtension(jsonFilePath));
                if (File.Exists(resourcePath))
                {
                    Tests.Add(new TestData(resourcePath, jsonFilePath));
                }
            }
        }

        [Test]
        public void TestConstructCompressedStream()
        {
            foreach (var testData in Tests)
            {
                if(ScriptMetadataResourceLegacy.IsLegacyVersion(testData.RecordInfo.VersionHash))
                {
                    continue;
                }

                var expectedJson = File.ReadAllText(testData.JsonFilePath);
                var expected = (ScriptMetadataResource)JsonConvert.DeserializeObject(expectedJson, ScriptMetadataResource.GetTypeFor(testData.RecordInfo.VersionHash));

                var compressedFileBytes = File.ReadAllBytes(testData.CompressedFilePath);

                using (var ms = new MemoryStream(compressedFileBytes))
                {
                    var actual = ScriptMetadataResource.Create(testData.RecordInfo.VersionHash);
                    actual.InitFromStream(ms);

                    Assert.AreEqual(expected.AssemblyTooltip, actual.AssemblyTooltip);
                    Assert.AreEqual(expected.DefaultScript, actual.DefaultScript);
                    Assert.AreEqual(expected.HasAssemblyTooltip, actual.HasAssemblyTooltip);
                    Assert.AreEqual(expected.UnknownA, actual.UnknownA);
                    Assert.AreEqual(expected.UnknownB, actual.UnknownB);
                    Assert.AreEqual(expected.UnknownC, actual.UnknownC);
                    Assert.AreEqual(expected.UnknownE, actual.UnknownE);
                    Assert.AreEqual(expected.Warnings, actual.Warnings);

                    Assert.AreEqual(expected.ScriptCount, actual.ScriptCount);
                    for (int i = 0; i < actual.ScriptCount; i++)
                    {
                        Assert.AreEqual(expected.Scripts[i].ClassName, actual.Scripts[i].ClassName);
                        Assert.AreEqual(expected.Scripts[i].UnknownF, actual.Scripts[i].UnknownF);
                        Assert.AreEqual(expected.Scripts[i].UnknownG, actual.Scripts[i].UnknownG);
                        Assert.AreEqual(expected.Scripts[i].UnknownH, actual.Scripts[i].UnknownH);
                        Assert.AreEqual(expected.Scripts[i].DisplayName, actual.Scripts[i].DisplayName);
                        Assert.AreEqual(expected.Scripts[i].Tooltip, actual.Scripts[i].Tooltip);

                        Assert.AreEqual(expected.Scripts[i].Properties.Count, actual.Scripts[i].Properties.Count);
                        for (var propertyIndex = 0; propertyIndex < actual.Scripts[i].Properties.Count; propertyIndex++)
                        {
                            var expectedProperty = expected.Scripts[i].Properties[propertyIndex];
                            var actualProperty = actual.Scripts[i].Properties[propertyIndex];

                            Assert.AreEqual(expectedProperty.Name, actualProperty.Name);
                            Assert.AreEqual(expectedProperty.Type, actualProperty.Type);

                            Assert.AreEqual(expectedProperty.Attributes.Count, actualProperty.Attributes.Count);
                            for (var attributeIndex = 0; attributeIndex < actualProperty.Attributes.Count; attributeIndex++)
                            {
                                var expectedAttribute = expectedProperty.Attributes[attributeIndex];
                                var actualAttribute = actualProperty.Attributes[attributeIndex];

                                Assert.AreEqual(expectedAttribute.Key,   actualAttribute.Key);
                                Assert.AreEqual(expectedAttribute.Value, actualAttribute.Value);
                            }
                        }
                    }

                    Assert.AreEqual(expected.Strings.Count, actual.Strings.Count);
                    for (var stringIndex = 0; stringIndex < actual.Strings.Count; stringIndex++)
                    {
                        Assert.AreEqual(expected.Strings[stringIndex].Key,   actual.Strings[stringIndex].Key);
                        Assert.AreEqual(expected.Strings[stringIndex].Value, actual.Strings[stringIndex].Value);
                    }
                }
            }
        }

        [Test]
        public void TestConstructCompressedStreamLegacy()
        {
            foreach (var testData in Tests)
            {
                if(ScriptMetadataResourceLegacy.IsLegacyVersion(testData.RecordInfo.VersionHash) == false)
                {
                    continue;
                }

                var expectedJson = File.ReadAllText(testData.JsonFilePath);
                var expected = (ScriptMetadataResourceLegacy)JsonConvert.DeserializeObject(expectedJson, ScriptMetadataResourceLegacy.GetTypeFor(testData.RecordInfo.VersionHash));

                var compressedFileBytes = File.ReadAllBytes(testData.CompressedFilePath);

                using (var ms = new MemoryStream(compressedFileBytes))
                {
                    var actual = ScriptMetadataResourceLegacy.Create(testData.RecordInfo.VersionHash);
                    actual.InitFromStream(ms);

                    Assert.AreEqual(expected.AssemblyName, actual.AssemblyName);
                    Assert.AreEqual(expected.Warnings, actual.Warnings);

                    Assert.AreEqual(expected.Properties.Count, actual.Properties.Count);
                    for (var propertyIndex = 0; propertyIndex < actual.Properties.Count; propertyIndex++)
                    {
                        var expectedProperty = expected.Properties[propertyIndex];
                        var actualProperty = actual.Properties[propertyIndex];

                        Assert.AreEqual(expectedProperty.Name, actualProperty.Name);
                        Assert.AreEqual(expectedProperty.Type, actualProperty.Type);

                        Assert.AreEqual(expectedProperty.Attributes.Count, actualProperty.Attributes.Count);
                        for (var attributeIndex = 0; attributeIndex < actualProperty.Attributes.Count; attributeIndex++)
                        {
                            var expectedAttribute = expectedProperty.Attributes[attributeIndex];
                            var actualAttribute = actualProperty.Attributes[attributeIndex];

                            Assert.AreEqual(expectedAttribute.Key,   actualAttribute.Key);
                            Assert.AreEqual(expectedAttribute.Value, actualAttribute.Value);
                        }
                    }

                    Assert.AreEqual(expected.Strings.Count, actual.Strings.Count);
                    for (var stringIndex = 0; stringIndex < actual.Strings.Count; stringIndex++)
                    {
                        Assert.AreEqual(expected.Strings[stringIndex].Key,   actual.Strings[stringIndex].Key);
                        Assert.AreEqual(expected.Strings[stringIndex].Value, actual.Strings[stringIndex].Value);
                    }
                }
            }
        }
    }
}
