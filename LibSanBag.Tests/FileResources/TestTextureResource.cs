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
    [TestFixture]
    internal class TestTextureResource : BaseFileResourceTest
    {
        private struct TestData
        {
            public string CompressedFilePath { get; set; }
            public string ExpectedCompressedPath { get; set; }
            public string ExpectedDecompressedPath { get; set; }
            public FileRecordInfo RecordInfo { get; set; }
            public List<string> SegmentedLevels { get; set; }

            public TestData(string compressedFilePath, string expectedCompressedPath, string expectedDecompressedPath, List<string> segmentedLevels)
            {
                CompressedFilePath = Path.Combine(RootPath, compressedFilePath);
                ExpectedCompressedPath = Path.Combine(RootPath, expectedCompressedPath);
                ExpectedDecompressedPath = Path.Combine(RootPath, expectedDecompressedPath);
                SegmentedLevels = segmentedLevels;

                RecordInfo = FileRecordInfo.Create(compressedFilePath);
            }
        }

        private static readonly string RootPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Resources", "Texture");
        private static readonly ulong PngHeader = 0x0a1a0a0d474e5089;

        private IEnumerable<TestData> Tests { get; } = new[]
        {
            new TestData(
                "4d0ab27f42b14326ed4987ed25566663.Texture-Resource.v9a8d4bbd19b4cd55.payload.v0.noVariants",
                "4d0ab27f42b14326ed4987ed25566663.Texture-Resource.v9a8d4bbd19b4cd55.payload.v0.noVariants.dds",
                "Sample.png",
                new List<string>(){ }
            ),
            new TestData(
                "7671009fd20711e4e4a0b81d500c2714.Texture-Resource.vbfc630a1f9234ffd.payload.v0.noVariants",
                "7671009fd20711e4e4a0b81d500c2714.Texture-Resource.vbfc630a1f9234ffd.payload.v0.noVariants.crn",
                "Sample.png",
                new List<string>(){ }
            ),
            new TestData(
                "076c8c42c7b5b9dceda66f233ac2630b.Texture-Resourcee.vbfc630a1f9234ffd.payload.v0.noVariants",
                "076c8c42c7b5b9dceda66f233ac2630b.Texture-Resourcee.vbfc630a1f9234ffd.payload.v0.noVariants.dds",
                "076c8c42c7b5b9dceda66f233ac2630b.Texture-Resourcee.vbfc630a1f9234ffd.payload.v0.noVariants.png",
                new List<string>(){ }
            ),
            new TestData(
                "88a56a0a29d778510778a259d9ba2c20.Texture-Resource.v2.payload.v0.noVariants",
                "88a56a0a29d778510778a259d9ba2c20.Texture-Resource.v2.payload.v0.noVariants.crn",
                "Sample.png",
                new List<string>(){ }
            ),
            new TestData(
                "7b1a5fa14c90a8e5fbd7918fa77e0c51.Texture-Resource.v2.payload.v0.noVariants",
                "7b1a5fa14c90a8e5fbd7918fa77e0c51.Texture-Resource.v2.payload.v0.noVariants.crn",
                "7b1a5fa14c90a8e5fbd7918fa77e0c51.Texture-Resource.v2.payload.v0.noVariants.png",
                new List<string>(){
                    "8ddc0a206075b8e7aecd85139ffc87a3"
                }
            ),
            new TestData(
                "b9ca06787a64475cc14dac63c49d4984.Texture-Resource.v2.payload.v0.noVariants",
                "b9ca06787a64475cc14dac63c49d4984.Texture-Resource.v2.payload.v0.noVariants.crn",
                "b9ca06787a64475cc14dac63c49d4984.Texture-Resource.v2.payload.v0.noVariants.png",
                new List<string>() {
                    "991060e46852e19729c1207d5d250273",
                    "dc3589b2f2bdefacb893346095f5dc46",
                }
            ),
        };

        [SetUp]
        public override void Setup()
        {
            base.Setup();

            if (LibDDS.IsAvailable == false)
            {
                Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
                LibDDS.FindDependencies(new FileProvider());
            }
        }

        [Test]
        public void TestConstructCompressedStream()
        {
            foreach (var testData in Tests)
            {
                var compressedFileBytes = File.ReadAllBytes(testData.CompressedFilePath);
                var expectedCompressedbytes = File.ReadAllBytes(testData.ExpectedCompressedPath);

                using (var ms = new MemoryStream(compressedFileBytes))
                {
                    var resource = TextureResource.Create(testData.RecordInfo.VersionHash);
                    resource.InitFromStream(ms);
                    Assert.AreEqual(expectedCompressedbytes, resource.CompressedTextureBytes);
                    Assert.AreEqual(testData.SegmentedLevels.Count, resource.StreamedMips.Count);

                    for (int i = 0; i < testData.SegmentedLevels.Count; i++)
                    {
                        Assert.AreEqual(testData.SegmentedLevels[i], resource.StreamedMips[i]);
                    }
                }
            }
        }

        [Test]
        public void TestConstructFileInfo()
        {
            foreach (var testData in Tests)
            {
                var expectedCompressedbytes = File.ReadAllBytes(testData.ExpectedCompressedPath);

                var fileStream = File.OpenRead(testData.CompressedFilePath);
                var fileRecord = new FileRecord
                {
                    Length = (uint)fileStream.Length,
                    Info = null,
                    Offset = 0,
                    TimestampNs = 0,
                    Name = "File Record"
                };

                var resource = TextureResource.Create(testData.RecordInfo.VersionHash);
                resource.InitFromRecord(fileStream, fileRecord);
                Assert.AreEqual(resource.CompressedTextureBytes, expectedCompressedbytes);
            }
        }

        [Test]
        public void TestConstructBytes()
        {
            foreach (var testData in Tests)
            {
                var expectedCompressedbytes = File.ReadAllBytes(testData.ExpectedCompressedPath);

                var filebytes = File.ReadAllBytes(testData.CompressedFilePath);

                var resource = TextureResource.Create(testData.RecordInfo.VersionHash);
                resource.InitFromRawCompressed(filebytes);
                Assert.AreEqual(resource.CompressedTextureBytes, expectedCompressedbytes);
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
                    var resource = TextureResource.Create(testData.RecordInfo.VersionHash);
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

                var resource = TextureResource.Create(testData.RecordInfo.VersionHash);
                resource.InitFromRawCompressed(filebytes);
                var imageBytes = resource.ConvertTo(TextureResource.TextureType.PNG);

                var header = BitConverter.ToUInt64(imageBytes, 0);
                Assert.AreEqual(PngHeader, header);
            }
        }
    }
}
