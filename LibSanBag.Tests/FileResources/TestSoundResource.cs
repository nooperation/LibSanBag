using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibSanBag.FileResources;
using NUnit.Framework;

namespace LibSanBag.Tests.FileResources
{
    class TestSoundResource
    {
        private struct TestData
        {
            public string CompressedFilePath { get; set; }
            public string ExpectedName { get; set; }
            public FileRecordInfo RecordInfo { get; set; }

            public TestData(string compressedFilePath, string expectedFileName)
            {
                CompressedFilePath = Path.Combine(RootPath, compressedFilePath);
                RecordInfo = FileRecordInfo.Create(compressedFilePath);
                ExpectedName = expectedFileName;
            }
        }

        private static readonly string RootPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Resources", "Sound");
        private static readonly string ExpectedFilePath = Path.Combine(RootPath, "Sample.fsb");

        private byte[] ExpectedSoundBytes { get; set; }
        private IEnumerable<TestData> Tests { get; } = new[]
        {
            new TestData("06996b132758196af622e23df4fe5811.Sound-Resource.v8510a121d70371a2.payload.v0.noVariants", "Sample"),
        };

        [SetUp]
        public void Setup()
        {
            ExpectedSoundBytes = File.ReadAllBytes(ExpectedFilePath);
        }

        [Test]
        public void TestConstructCompressedStream()
        {
            foreach (var testData in Tests)
            {
                var compressedFileBytes = File.ReadAllBytes(testData.CompressedFilePath);

                using (var ms = new MemoryStream(compressedFileBytes))
                {
                    var resource = SoundResource.Create(testData.RecordInfo.VersionHash);
                    resource.InitFromStream(ms);
                    Assert.AreEqual(resource.Name, testData.ExpectedName);
                    Assert.AreEqual(resource.SoundBytes, ExpectedSoundBytes);
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

                var resource = SoundResource.Create(testData.RecordInfo.VersionHash);
                resource.InitFromRecord(fileStream, fileRecord);
                Assert.AreEqual(resource.Name, testData.ExpectedName);
                Assert.AreEqual(resource.SoundBytes, ExpectedSoundBytes);
            }
        }

        [Test]
        public void TestConstructBytes()
        {
            foreach (var testData in Tests)
            {
                var filebytes = File.ReadAllBytes(testData.CompressedFilePath);

                var resource = SoundResource.Create(testData.RecordInfo.VersionHash);
                resource.InitFromRawCompressed(filebytes);
                Assert.AreEqual(resource.Name, testData.ExpectedName);
                Assert.AreEqual(resource.SoundBytes, ExpectedSoundBytes);
            }
        }
    }
}
