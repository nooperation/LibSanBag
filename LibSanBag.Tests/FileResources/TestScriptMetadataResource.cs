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
    class TestScriptMetadataResource
    {
        private struct TestData
        {
            public string CompressedFilePath { get; }
            public ScriptMetadataResource Expected { get; }
            public FileRecordInfo RecordInfo { get; }

            public TestData(string compressedFilePath, string jsonFilePath)
            {
                CompressedFilePath = Path.Combine(RootPath, compressedFilePath);
                RecordInfo = FileRecordInfo.Create(compressedFilePath);

                var expectedJson = File.ReadAllText(jsonFilePath);
                Expected = JsonConvert.DeserializeObject<ScriptMetadataResourceV3>(expectedJson);
            }
        }

        private static readonly string RootPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Resources", "ScriptMetadata");
        private static readonly List<TestData> Tests = new List<TestData>();

        [SetUp]
        public void Setup()
        {
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
                var compressedFileBytes = File.ReadAllBytes(testData.CompressedFilePath);
                var expected = testData.Expected;

                using (var ms = new MemoryStream(compressedFileBytes))
                {
                    var actual = ScriptMetadataResource.Create(testData.RecordInfo.VersionHash);
                    actual.InitFromStream(ms);

                    Assert.AreEqual(expected.AssemblyName, actual.AssemblyName);
                    Assert.AreEqual(expected.Warnings, actual.Warnings);

                    Assert.AreEqual(expected.Properties.Count, actual.Properties.Count);
                    for (int i = 0; i < actual.Properties.Count; i++)
                    {
                        Assert.AreEqual(expected.Properties[i].Name, actual.Properties[i].Name);
                        Assert.AreEqual(expected.Properties[i].Type, actual.Properties[i].Type);

                        Assert.AreEqual(expected.Properties[i].Attributes.Count, actual.Properties[i].Attributes.Count);
                        for (int j = 0; j < actual.Properties[i].Attributes.Count; j++)
                        {
                            Assert.AreEqual(expected.Properties[i].Attributes[j].Key, actual.Properties[i].Attributes[j].Key);
                            Assert.AreEqual(expected.Properties[i].Attributes[j].Value, actual.Properties[i].Attributes[j].Value);
                        }
                    }

                    Assert.AreEqual(expected.Strings.Count, actual.Strings.Count);
                    for (int j = 0; j < actual.Strings.Count; j++)
                    {
                        Assert.AreEqual(actual.Strings[j].Key,   actual.Strings[j].Key);
                        Assert.AreEqual(actual.Strings[j].Value, actual.Strings[j].Value);
                    }
                }
            }
        }
    }
}
