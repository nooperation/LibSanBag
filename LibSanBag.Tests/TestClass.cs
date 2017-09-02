using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace LibSanBag.Tests
{
    [TestFixture]
    public class TestClass
    {
        private Mock<ITimeProvider> MockTimeProvider = new Mock<ITimeProvider>();
        private ulong ExpectedTimestamp => 0x14DF773F94417DC0;

        [SetUp]
        public void Setup()
        {
            MockTimeProvider.Setup(func => func.GetCurrentTime()).Returns(ExpectedTimestamp);
        }

        [Test]
        public void TestCreateEmptyBag()
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                Bag.Write(outStream, new List<string>(), MockTimeProvider.Object);
                Assert.AreEqual(outStream.ToArray(), ExpectedData.EmptyBag);
            }
        }

        [Test]
        public void TestCreateSingleFileBag()
        {
            var filesToAdd = new string[]
            {
                Path.Combine(TestContext.CurrentContext.TestDirectory, "in", "TestFile1.txt")
            };

            using (MemoryStream outStream = new MemoryStream())
            {
                Bag.Write(outStream, filesToAdd, MockTimeProvider.Object);
                Assert.AreEqual(outStream.ToArray(), ExpectedData.SingleFile);
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

            using (MemoryStream outStream = new MemoryStream())
            {
                Bag.Write(outStream, filesToAdd, MockTimeProvider.Object);
                Assert.AreEqual(outStream.ToArray(), ExpectedData.MultipleFiles);
            }
        }

        [Test]
        public void TestReadEmptyBag()
        {
            using (MemoryStream inStream = new MemoryStream(ExpectedData.EmptyBag))
            {
                var files = Bag.Read(inStream);
                Assert.IsEmpty(files);
            }
        }

        [Test]
        public void TestReadSingleFileBag()
        {
            using (MemoryStream inStream = new MemoryStream(ExpectedData.SingleFile))
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
            using (MemoryStream inStream = new MemoryStream(ExpectedData.MultipleFiles))
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
            using (MemoryStream inStream = new MemoryStream(ExpectedData.SingleFile))
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
            using (MemoryStream inStream = new MemoryStream(ExpectedData.MultipleFiles))
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
    }
}
