using System.Collections.Generic;
using System.IO;
using LibSanBag.FileResources;
using Newtonsoft.Json;
using NUnit.Framework;

namespace LibSanBag.Tests.FileResources
{
    [TestFixture]
    internal class TestSoundResource : BaseFileResourceTest
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

        private static readonly string RootPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Resources", "Sound");
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
                var compressedFileBytes = File.ReadAllBytes(testData.CompressedFilePath);

                using (var ms = new MemoryStream(compressedFileBytes))
                {
                    var actual = ScriptMetadataResource.Create(testData.RecordInfo.VersionHash);
                    actual.InitFromStream(ms);

                    var actualJson = JsonConvert.SerializeObject(actual);

                    Assert.AreEqual(expectedJson, actualJson);
                }
            }
        }
    }
}
