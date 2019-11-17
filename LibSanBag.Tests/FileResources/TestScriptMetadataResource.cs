using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibSanBag.FileResources;
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
                var expectedJson = File.ReadAllText(testData.JsonFilePath);
                var expected = (ScriptMetadataResource)JsonConvert.DeserializeObject(expectedJson, ScriptMetadataResource.GetTypeFor(testData.RecordInfo.VersionHash));

                var compressedFileBytes = File.ReadAllBytes(testData.CompressedFilePath);

                using (var ms = new MemoryStream(compressedFileBytes))
                {
                    var actual = ScriptMetadataResource.Create(testData.RecordInfo.VersionHash);
                    actual.InitFromStream(ms);

                    Assert.AreEqual(expected.AssemblyTooltip, actual.AssemblyTooltip);
                    Assert.AreEqual(expected.ScriptSourceTextName, actual.ScriptSourceTextName);
                    Assert.AreEqual(expected.BuildWarnings, actual.BuildWarnings);
                    Assert.AreEqual(expected.DefaultScript, actual.DefaultScript);
                    Assert.AreEqual(expected.OtherWarnings, actual.OtherWarnings);
                    Assert.AreEqual(expected.UsesRestrictedAPI, actual.UsesRestrictedAPI);
                    Assert.AreEqual(expected.ScriptCount, actual.ScriptCount);

                    Assert.AreEqual(expected.AttributesVersion, actual.AttributesVersion);
                    Assert.AreEqual(expected.AttributeVersion, actual.AttributeVersion);
                    Assert.AreEqual(expected.PropertyVersion, actual.PropertyVersion);
                    Assert.AreEqual(expected.ResourceVersion, actual.ResourceVersion);
                    Assert.AreEqual(expected.ScriptPayloadVersion, actual.ScriptPayloadVersion);
                    Assert.AreEqual(expected.ScriptsVersion, actual.ScriptsVersion);
                    Assert.AreEqual(expected.ScriptVersion, actual.ScriptVersion);

                    Assert.AreEqual(expected.Scripts.Count, actual.Scripts.Count);
                    for (int i = 0; i < actual.Scripts.Count; i++)
                    {
                        var expectedScript = expected.Scripts[i];
                        var actualScript = actual.Scripts[i];

                        Assert.AreEqual(expectedScript.ClassName, actualScript.ClassName);
                        Assert.AreEqual(expectedScript.DisplayName, actualScript.DisplayName);
                        Assert.AreEqual(expectedScript.Tooltip, actualScript.Tooltip);
                        Assert.AreEqual(expectedScript.UnknownA, actualScript.UnknownA);

                        Assert.AreEqual(expectedScript.Properties.Count, actualScript.Properties.Count);
                        for(int propIndex = 0; propIndex < actualScript.Properties.Count; ++propIndex)
                        {
                            var expectedProperty = expectedScript.Properties[propIndex];
                            var actualProperty = actualScript.Properties[propIndex];

                            Assert.AreEqual(expectedProperty.Name, actualProperty.Name);
                            Assert.AreEqual(expectedProperty.Type, actualProperty.Type);

                            Assert.AreEqual(expectedProperty.Attributes.Count, actualProperty.Attributes.Count);
                            for(int attributeIndex = 0; attributeIndex < actualProperty.Attributes.Count; ++ attributeIndex)
                            {
                                var expectedAttribute = expectedProperty.Attributes[attributeIndex];
                                var actualAttribute = actualProperty.Attributes[attributeIndex];

                                Assert.AreEqual(expectedAttribute.Name, actualAttribute.Name);
                                Assert.AreEqual(expectedAttribute.Type, actualAttribute.Type);
                                Assert.AreEqual(expectedAttribute.Value, actualAttribute.Value);
                            }

                        }
                    }
                }
            }
        }
    }
}
