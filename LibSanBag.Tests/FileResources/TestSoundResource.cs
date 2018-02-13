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
        private byte[] expectedSoundBytes;

        private string CompressedFilePath => Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Resources", "Sound", "06996b132758196af622e23df4fe5811.Sound-Resource.v8510a121d70371a2.payload.v0.noVariants");
        private string ExpectedFilePath => Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Resources", "Sound", "Sample.fsb");
        private string ExpectedName => "enterprise";

        [SetUp]
        public void Setup()
        {
            expectedSoundBytes = File.ReadAllBytes(ExpectedFilePath);
        }

        [Test]
        public void TestConstructCompressedStream()
        {
            var compressedFileBytes = File.ReadAllBytes(CompressedFilePath);

            using (var ms = new MemoryStream(compressedFileBytes))
            {
                var resource = SoundResource.Create();
                resource.InitFromStream(ms);
                Assert.AreEqual(resource.Name, ExpectedName);
                Assert.AreEqual(resource.SoundBytes, expectedSoundBytes);
            }
        }

        [Test]
        public void TestConstructFileInfo()
        {
            var fileStream = File.OpenRead(CompressedFilePath);
            var fileRecord = new FileRecord
            {
                Length = (uint)fileStream.Length,
                Info = null,
                Offset = 0,
                TimestampNs = 0,
                Name = "File Record"
            };

            var resource = SoundResource.Create();
            resource.InitFromRecord(fileStream, fileRecord);
            Assert.AreEqual(resource.Name, ExpectedName);
            Assert.AreEqual(resource.SoundBytes, expectedSoundBytes);
        }

        [Test]
        public void TestConstructBytes()
        {
            var filebytes = File.ReadAllBytes(CompressedFilePath);

            var resource = SoundResource.Create();
            resource.InitFromRawCompressed(filebytes);
            Assert.AreEqual(resource.Name, ExpectedName);
            Assert.AreEqual(resource.SoundBytes, expectedSoundBytes);
        }
    }
}
