using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibSanBag.Providers;
using Moq;

namespace LibSanBag.Tests
{
    [TestFixture]
    public class TestClass
    {
        private Mock<ITimeProvider> MockTimeProvider = new Mock<ITimeProvider>();
        private ulong ExpectedTimestamp => 0x14DF773F94417DC0;

        private string EmptyBagPath => Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Bag", "Empty.bag");
        private string SingleFileBagPath => Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Bag", "SingleFile.bag");
        private string MultipleFileBagPath => Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Bag", "MultipleFile.bag");
        private string InvalidHeaderBagPath => Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Bag", "InvalidHeader.bag");
        private string InvalidPaddingBagPath => Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Bag", "InvalidPadding.bag");
        private string InvalidNextManifestLengthBagPath => Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Bag", "InvalidNextManifestLength.bag");
        private string InvalidNextManifestOffsetBagPath => Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Bag", "InvalidNextManifestOffset.bag");
        private string InvalidFileOffsetBagPath => Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Bag", "InvalidFileOffset.bag");
        private string InvalidFileLengthBagPath => Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Bag", "InvalidFileLength.bag");

        [SetUp]
        public void Setup()
        {
            MockTimeProvider.Setup(func => func.GetCurrentTime()).Returns(ExpectedTimestamp);
        }

        [Test]
        public void TestCreateEmptyBag()
        {
            var expectedBytes = File.ReadAllBytes(EmptyBagPath);

            using (MemoryStream outStream = new MemoryStream())
            {
                Bag.Write(outStream, new List<string>(), MockTimeProvider.Object);
                Assert.AreEqual(outStream.ToArray(), expectedBytes);
            }
        }

        [Test]
        public void TestCreateSingleFileBag()
        {
            var filesToAdd = new string[]
            {
                Path.Combine(TestContext.CurrentContext.TestDirectory, "in", "TestFile1.txt")
            };

            var expectedBytes = File.ReadAllBytes(SingleFileBagPath);

            using (MemoryStream outStream = new MemoryStream())
            {
                Bag.Write(outStream, filesToAdd, MockTimeProvider.Object);
                Assert.AreEqual(outStream.ToArray(), expectedBytes);
            }
        }

        [Test]
        public void TestCreateMultipleFileBag()
        {
            var filesToAdd = new string[]
            {
                Path.Combine(TestContext.CurrentContext.TestDirectory, "in", "TestFile1.txt"),
                Path.Combine(TestContext.CurrentContext.TestDirectory, "in", "TestFile2.txt")
            };

            var expectedBytes = File.ReadAllBytes(MultipleFileBagPath);

            using (MemoryStream outStream = new MemoryStream())
            {
                Bag.Write(outStream, filesToAdd, MockTimeProvider.Object);
                Assert.AreEqual(outStream.ToArray(), expectedBytes);
            }
        }

        [Test]
        public void TestReadEmptyBag()
        {
            using (var inStream = File.OpenRead(EmptyBagPath))
            {
                var files = Bag.Read(inStream);
                Assert.IsEmpty(files);
            }
        }

        [Test]
        public void TestInvalidHeaderBag()
        {
            using (var inStream = File.OpenRead(InvalidHeaderBagPath))
            {
                Assert.Throws<Exception>(() => Bag.Read(inStream));
            }
        }

        [Test]
        public void TestInvalidPaddingBag()
        {
            using (var inStream = File.OpenRead(InvalidPaddingBagPath))
            {
                Assert.Throws<Exception>(() => Bag.Read(inStream));
            }
        }

        [Test]
        public void TestInvalidNextManifestLengthBag()
        {
            using (var inStream = File.OpenRead(InvalidNextManifestLengthBagPath))
            {
                Assert.Throws<Exception>(() => Bag.Read(inStream));
            }
        }

        [Test]
        public void TestInvalidNextManifestOffsetBag()
        {
            using (var inStream = File.OpenRead(InvalidNextManifestOffsetBagPath))
            {
                Assert.Throws<Exception>(() => Bag.Read(inStream));
            }
        }

        [Test]
        public void TestReadSingleFileBag()
        {
            using (var inStream = File.OpenRead(SingleFileBagPath))
            {
                var files = Bag.Read(inStream).ToList();
                Assert.AreEqual(files.Count, 1);

                Assert.AreEqual(files[0].Length, 0x0F);
                Assert.AreEqual(files[0].Name, "TestFile1.txt");
                Assert.AreEqual(files[0].TimestampNs, ExpectedTimestamp);
                Assert.AreEqual(files[0].Offset, 0x432);
            }
        }

        [Test]
        public void TestReadMultipleFileBag()
        {
            using (var inStream = File.OpenRead(MultipleFileBagPath))
            {
                var files = Bag.Read(inStream).ToList();
                Assert.AreEqual(files.Count, 2);

                Assert.AreEqual(files[0].Length, 0x0F);
                Assert.AreEqual(files[0].Name, "TestFile1.txt");
                Assert.AreEqual(files[0].Offset, 0x458);
                Assert.AreEqual(files[0].TimestampNs, ExpectedTimestamp);

                Assert.AreEqual(files[1].Length, 0x10);
                Assert.AreEqual(files[1].Name, "TestFile2.txt");
                Assert.AreEqual(files[1].Offset, 0x467);
                Assert.AreEqual(files[1].TimestampNs, ExpectedTimestamp);
            }
        }

        [Test]
        public void TestExtractSingleFile()
        {
            using (var inStream = File.OpenRead(SingleFileBagPath))
            {
                var files = Bag.Read(inStream).ToList();
                using (var outStream = new MemoryStream())
                {
                    files[0].Save(inStream, outStream);
                    var buffer = outStream.ToArray();
                    var content = Encoding.ASCII.GetString(buffer);

                    Assert.AreEqual(content, "First Test File");
                }
            }
        }

        [Test]
        public void TestExtractMultipleFile()
        {
            using (var inStream = File.OpenRead(MultipleFileBagPath))
            {
                var expectedContent = new string[]
                {
                    "First Test File",
                    "Second Test File"
                };

                var records = Bag.Read(inStream).ToList();
                for(int i = 0; i < records.Count; ++i)
                {
                    using (var outStream = new MemoryStream())
                    {
                        records[i].Save(inStream, outStream);
                        var buffer = outStream.ToArray();
                        var content = Encoding.ASCII.GetString(buffer);

                        Assert.AreEqual(content, expectedContent[i]);
                    }
                }
            }
        }

        [Test]
        public void TestExtractFileCancel()
        {
            using (var inStream = File.OpenRead(SingleFileBagPath))
            {
                var files = Bag.Read(inStream).ToList();
                using (var outStream = new MemoryStream())
                {
                    Assert.Throws<Exception>(() =>
                    {
                        files[0].Save(inStream, outStream, null, () => true);
                    });
                }
            }
        }

        [Test]
        public void TestExtractInvalidFileOffset()
        {
            using (var inStream = File.OpenRead(InvalidFileOffsetBagPath))
            {
                var files = Bag.Read(inStream).ToList();
                using (var outStream = new MemoryStream())
                {
                    Assert.Throws<Exception>(() =>
                    {
                        files[0].Save(inStream, outStream);
                    });
                }
            }
        }

        [Test]
        public void TestExtractInvalidFileLength()
        {
            using (var inStream = File.OpenRead(InvalidFileLengthBagPath))
            {
                var files = Bag.Read(inStream).ToList();
                using (var outStream = new MemoryStream())
                {
                    Assert.Throws<Exception>(() =>
                    {
                        files[0].Save(inStream, outStream);
                    });
                }
            }
        }

        [Test]
        public void TestToStringWorks()
        {
            using (var inStream = File.OpenRead(SingleFileBagPath))
            {
                var files = Bag.Read(inStream).ToList();
                using (var outStream = new MemoryStream())
                {
                    files[0].Info = null;

                    Assert.DoesNotThrow(() =>
                    {
                        files[0].ToString();
                    });

                    files[0].Info = FileRecordInfo.Create($"0f74af948a58a66a96f4fc282a01ebf1.Texture-Resource.v9a8d4bbd19b4cd55.payload.v0.noVariants");

                    Assert.DoesNotThrow(() =>
                    {
                        files[0].ToString();
                    });
                }
            }
        }
    }
}
