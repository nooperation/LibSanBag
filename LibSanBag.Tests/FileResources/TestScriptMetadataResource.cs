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
                //var expected = (ScriptMetadataResource)JsonConvert.DeserializeObject(expectedJson, ScriptMetadataResource.GetTypeFor(testData.RecordInfo.VersionHash));

                var compressedFileBytes = File.ReadAllBytes(testData.CompressedFilePath);

                
                using (var ms = new MemoryStream(compressedFileBytes))
                {
                    var actual = ScriptMetadataResource.Create(testData.RecordInfo.VersionHash);
                    actual.InitFromStream(ms);

                    var actualJson = JsonConvert.SerializeObject(actual);

                    Assert.AreEqual(expectedJson, actualJson);

                    /*
                    Assert.AreEqual(expected.Resource.Tooltip, actual.Resource.Tooltip);
                    Assert.AreEqual(expected.Resource.SourceFileName, actual.Resource.SourceFileName);
                    Assert.AreEqual(expected.Resource.Info, actual.Resource.Info);
                    Assert.AreEqual(expected.Resource.DefaultScript, actual.Resource.DefaultScript);
                    Assert.AreEqual(expected.Resource.Errors, actual.Resource.Errors);
                    Assert.AreEqual(expected.Resource.Flags, actual.Resource.Flags);
                    Assert.AreEqual(expected.Resource.Version, actual.Resource.Version);

                    Assert.AreEqual(expected.Resource.Tags.Count, actual.Resource.Tags.Count);
                    for (int i = 0; i < actual.Resource.Tags.Count; i++)
                    {
                        var expectedItem = expected.Resource.Tags[i];
                        var actualItem = actual.Resource.Tags[i];

                        Assert.AreEqual(expectedItem.Data, actualItem.Data);
                        Assert.AreEqual(expectedItem.Value, actualItem.Value);
                    }

                    Assert.AreEqual(expected.Resource.Parameters.Count, actual.Resource.Parameters.Count);
                    for (int i = 0; i < actual.Resource.Parameters.Count; i++)
                    {
                        var expectedItem = expected.Resource.Parameters[i];
                        var actualItem = actual.Resource.Parameters[i];

                        Assert.AreEqual(expectedItem.Key, actualItem.Key);
                        Assert.AreEqual(expectedItem.Value, actualItem.Value);
                    }

                    Assert.AreEqual(expected.Resource.Properties.Count, actual.Resource.Properties.Count);
                    for (int i = 0; i < actual.Resource.Properties.Count; i++)
                    {
                        var expectedItem = expected.Resource.Properties[i];
                        var actualItem = actual.Resource.Properties[i];

                        Assert.AreEqual(expectedItem.Version, actualItem.Version);
                        Assert.AreEqual(expectedItem.Name, actualItem.Name);
                        Assert.AreEqual(expectedItem.TypeString, actualItem.TypeString);
                        Assert.AreEqual(expectedItem.Type, actualItem.Type);

                        Assert.AreEqual(expectedItem.Attributes.Count, actualItem.Attributes.Count);
                        for (int j = 0; j < actualItem.Attributes.Count; j++)
                        {
                            var expectedAttribute = expectedItem.Attributes[i];
                            var actualAttribute = actualItem.Attributes[i];


                        }
                    }

                    Assert.AreEqual(expected.Resource.ScriptClasses.Count, actual.Resource.ScriptClasses.Count);
                    for (int i = 0; i < actual.Resource.ScriptClasses.Count; i++)
                    {
                        var expectedScript = expected.Resource.ScriptClasses[i];
                        var actualScript = actual.Resource.ScriptClasses[i];

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
                    }*/
                }
            }
        }
    }
}
