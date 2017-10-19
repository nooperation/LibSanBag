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
    class TestTextureResource
    {
        byte[] expectedTextureBytes;

        [SetUp]
        public void Setup()
        {
            expectedTextureBytes = File.ReadAllBytes(Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Texture-Resource.dds"));
        }

        [Test]
        public void TestConstructCompressedStream()
        {
            var compressedFileBytes = File.ReadAllBytes(Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Texture-Resource.bin"));

            using (var ms = new MemoryStream(compressedFileBytes))
            {
                var resource = new TextureResource(ms);
                Assert.AreEqual(resource.DdsBytes, expectedTextureBytes);
            }
        }

        [Test]
        public void TestConstructFileInfo()
        {
            var fileStream = File.OpenRead(Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Texture-Resource.bin"));
            var fileRecord = new FileRecord
            {
                Length = (uint)fileStream.Length,
                Info = null,
                Offset = 0,
                TimestampNs = 0,
                Name = "File Record"
            };

            var resource = new TextureResource(fileStream, fileRecord);
            Assert.AreEqual(resource.DdsBytes, expectedTextureBytes);
        }

        [Test]
        public void TestConstructBytes()
        {
            var filebytes = File.ReadAllBytes(Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Texture-Resource.bin"));

            var resource = new TextureResource(filebytes);
            Assert.AreEqual(resource.DdsBytes, expectedTextureBytes);
        }
    }
}
