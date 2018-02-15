using LibSanBag.FileResources;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibSanBag.Providers;
using LibSanBag.ResourceUtils;

namespace LibSanBag.Tests.FileResources
{
    class TestTextureResource
    {
        private struct TestData
        {
            public string CompressedFilePath { get; set; }
        }

        private static readonly string RootPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Resources", "Texture");
        private static readonly string ExpectedFilePath = Path.Combine(RootPath, "Texture-Resource.dds");
        private static readonly ulong PngHeader = 0x0a1a0a0d474e5089;

        private byte[] ExpectedBytes { get; set; }
        private IEnumerable<TestData> Tests { get; } = new[]
        {
            new TestData
            {
                CompressedFilePath = Path.Combine(RootPath, "4d0ab27f42b14326ed4987ed25566663.Texture-Resource.v9a8d4bbd19b4cd55.payload.v0.noVariants"),
            }
        };

        [SetUp]
        public void Setup()
        {
            if (LibDDS.IsAvailable == false)
            {
                Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
                LibDDS.FindDependencies(new FileProvider());
            }

            ExpectedBytes = File.ReadAllBytes(ExpectedFilePath);
        }

        [Test]
        public void TestConstructCompressedStream()
        {
            foreach (var testData in Tests)
            {
                var compressedFileBytes = File.ReadAllBytes(testData.CompressedFilePath);

                using (var ms = new MemoryStream(compressedFileBytes))
                {
                    var resource = TextureResource.Create();
                    resource.InitFromStream(ms);
                    Assert.AreEqual(resource.DdsBytes, ExpectedBytes);
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

                var resource = TextureResource.Create();
                resource.InitFromRecord(fileStream, fileRecord);
                Assert.AreEqual(resource.DdsBytes, ExpectedBytes);
            }
        }

        [Test]
        public void TestConstructBytes()
        {
            foreach (var testData in Tests)
            {
                var filebytes = File.ReadAllBytes(testData.CompressedFilePath);

                var resource = TextureResource.Create();
                resource.InitFromRawCompressed(filebytes);
                Assert.AreEqual(resource.DdsBytes, ExpectedBytes);
            }
        }

        [Test]
        public void TestBadDdsHeader()
        {
            foreach (var testData in Tests)
            {
                var badDdsHeaderPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Resources", "Texture", "Texture-ResourceBadDdsHeader.bin");
                var filebytes = File.ReadAllBytes(badDdsHeaderPath);

                Assert.Throws<Exception>(() =>
                {
                    var resource = TextureResource.Create();
                    resource.InitFromRawCompressed(filebytes);
                });
            }
        }

        [Test]
        public void TestConvertTo()
        {
            foreach (var testData in Tests)
            {
                var filebytes = File.ReadAllBytes(testData.CompressedFilePath);

                var resource = TextureResource.Create();
                resource.InitFromRawCompressed(filebytes);
                var imageBytes = resource.ConvertTo(ResourceUtils.LibDDS.ConversionOptions.CodecType.CODEC_PNG);

                var header = BitConverter.ToUInt64(imageBytes, 0);
                Assert.AreEqual(PngHeader, header);
            }
        }
    }
}
