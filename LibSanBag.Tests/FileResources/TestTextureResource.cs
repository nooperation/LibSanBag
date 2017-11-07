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
        private byte[] expectedTextureBytes;
        public ulong PngHeader => 0x0a1a0a0d474e5089;

        private string CompressedFilePath => Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Texture-Resource.bin");
        private string ExpectedFilePath => Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Texture-Resource.dds");

        [SetUp]
        public void Setup()
        {
            expectedTextureBytes = File.ReadAllBytes(ExpectedFilePath);
        }

        [Test]
        public void TestConstructCompressedStream()
        {
            var compressedFileBytes = File.ReadAllBytes(CompressedFilePath);

            using (var ms = new MemoryStream(compressedFileBytes))
            {
                var resource = new TextureResource(ms);
                Assert.AreEqual(resource.DdsBytes, expectedTextureBytes);
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

            var resource = new TextureResource(fileStream, fileRecord);
            Assert.AreEqual(resource.DdsBytes, expectedTextureBytes);
        }

        [Test]
        public void TestConstructBytes()
        {
            var filebytes = File.ReadAllBytes(CompressedFilePath);

            var resource = new TextureResource(filebytes);
            Assert.AreEqual(resource.DdsBytes, expectedTextureBytes);
        }

        [Test]
        public void TestBadDdsHeader()
        {
            var badDdsHeaderPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Texture-ResourceBadDdsHeader.bin");
            var filebytes = File.ReadAllBytes(badDdsHeaderPath);

            Assert.Throws<Exception>(() =>
            {
                var resource = new TextureResource(filebytes);
            });
        }

        [Test]
        public void TestConvertTo()
        {
            var filebytes = File.ReadAllBytes(CompressedFilePath);

            var resource = new TextureResource(filebytes);
            var imageBytes = resource.ConvertTo(ResourceUtils.LibDDS.ConversionOptions.CodecType.CODEC_PNG);

            var header = BitConverter.ToUInt64(imageBytes, 0);
            Assert.AreEqual(PngHeader, header);
        }
    }
}
