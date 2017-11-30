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

        private string CompressedFilePath => Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Sound-Resource.bin");
        private string ExpectedFilePath => Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Sound-Resource.fsb");
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
                var resource = new SoundResource(ms);
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

            var resource = new SoundResource(fileStream, fileRecord);
            Assert.AreEqual(resource.Name, ExpectedName);
            Assert.AreEqual(resource.SoundBytes, expectedSoundBytes);
        }

        [Test]
        public void TestConstructBytes()
        {
            var filebytes = File.ReadAllBytes(CompressedFilePath);

            var resource = new SoundResource(filebytes);
            Assert.AreEqual(resource.Name, ExpectedName);
            Assert.AreEqual(resource.SoundBytes, expectedSoundBytes);
        }
    }
}
