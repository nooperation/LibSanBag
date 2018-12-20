using LibSanBag.FileResources;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSanBag.Tests.FileResources
{
    [TestFixture]
    internal class TestScriptSourceTextResource : BaseFileResourceTest
    {
        private struct TestData
        {
            public string CompressedFilePath { get; set; }
            public string ExpectedFileName { get; set; }
            public FileRecordInfo RecordInfo { get; set; }

            public TestData(string compressedFilePath, string expectedFileName)
            {
                CompressedFilePath = Path.Combine(RootPath, compressedFilePath);
                RecordInfo = FileRecordInfo.Create(compressedFilePath);
                ExpectedFileName = expectedFileName;
            }
        }

        private static readonly string RootPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Resources", "ScriptSourceText");
        private static readonly string ExpectedFilePath = Path.Combine(RootPath, "ScriptSourceText-Resource.cs");

        private string ExpectedSource { get; set; }
        private IEnumerable<TestData> Tests { get; } = new[]
        {
            new TestData("63d3d75933432b36adca64c6d778a1d7.ScriptSourceText-Resource.v6301a7d31aa6f628.payload.v0.noVariants", "ExampleScript.cs"),
            new TestData("4b6e1436d155d91deeab038bb225ab21.ScriptSourceText-Resource.v4cde67396803610f.payload.v0.noVariants", string.Empty),
        };

        [SetUp]
        public override void Setup()
        {
            base.Setup();

            ExpectedSource = File.ReadAllText(ExpectedFilePath);
        }

        [Test]
        public void TestConstructCompressedStream()
        {
            foreach (var testData in Tests)
            {

                var compressedFileBytes = File.ReadAllBytes(testData.CompressedFilePath);

                using (var ms = new MemoryStream(compressedFileBytes))
                {
                    var resource = ScriptSourceTextResource.Create(testData.RecordInfo.VersionHash);
                    resource.InitFromStream(ms);
                    Assert.AreEqual(resource.Filename, testData.ExpectedFileName);
                    Assert.AreEqual(resource.Source, ExpectedSource);
                }
            }
        }

        [Test]
        public void TestConstructFileInfo()
        {
            foreach (var testData in Tests)
            {

                var fileStream = File.OpenRead(testData.CompressedFilePath);
                var fileRecord = new FileRecord
                {
                    Length = (uint)fileStream.Length,
                    Info = null,
                    Offset = 0,
                    TimestampNs = 0,
                    Name = "File Record"
                };

                var resource = ScriptSourceTextResource.Create(testData.RecordInfo.VersionHash);
                resource.InitFromRecord(fileStream, fileRecord);
                Assert.AreEqual(resource.Filename, testData.ExpectedFileName);
                Assert.AreEqual(resource.Source, ExpectedSource);
            }
        }

        [Test]
        public void TestConstructBytes()
        {
            foreach (var testData in Tests)
            {
                var filebytes = File.ReadAllBytes(testData.CompressedFilePath);

                var resource = ScriptSourceTextResource.Create(testData.RecordInfo.VersionHash);
                resource.InitFromRawCompressed(filebytes);
                Assert.AreEqual(resource.Filename, testData.ExpectedFileName);
                Assert.AreEqual(resource.Source, ExpectedSource);
            }
        }
    }
}
