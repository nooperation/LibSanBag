using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace SanBag.Tests
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
            using (MemoryStream out_stream = new MemoryStream())
            {
                Bag.Write(out_stream, new List<string>(), MockTimeProvider.Object);
                Assert.AreEqual(out_stream.ToArray(), ExpectedData.EmptyBag);
            }
        }

        [Test]
        public void TestCreateSingleFileBag()
        {
            var files_to_add = new string[]
            {
                Path.Combine(TestContext.CurrentContext.TestDirectory, "in", "TestFile1.txt")
            };

            using (MemoryStream out_stream = new MemoryStream())
            {
                Bag.Write(out_stream, files_to_add, MockTimeProvider.Object);
                Assert.AreEqual(out_stream.ToArray(), ExpectedData.SingleFile);
            }
        }

        [Test]
        public void TestCreateMultipleFileBag()
        {
            var files_to_add = new string[]
            {
                Path.Combine(TestContext.CurrentContext.TestDirectory, "in", "TestFile1.txt"),
                Path.Combine(TestContext.CurrentContext.TestDirectory, "in", "TestFile2.txt")
            };

            using (MemoryStream out_stream = new MemoryStream())
            {
                Bag.Write(out_stream, files_to_add, MockTimeProvider.Object);
                Assert.AreEqual(out_stream.ToArray(), ExpectedData.MultipleFiles);
            }
        }

        [Test]
        public void TestReadEmptyBag()
        {
            using (MemoryStream in_stream = new MemoryStream(ExpectedData.EmptyBag))
            {
                var files = Bag.Read(in_stream);
                Assert.IsEmpty(files);
            }
        }

        [Test]
        public void TestReadSingleFileBag()
        {
            using (MemoryStream in_stream = new MemoryStream(ExpectedData.SingleFile))
            {
                var files = Bag.Read(in_stream).ToList();
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
            using (MemoryStream in_stream = new MemoryStream(ExpectedData.MultipleFiles))
            {
                var files = Bag.Read(in_stream).ToList();
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
            using (MemoryStream in_stream = new MemoryStream(ExpectedData.SingleFile))
            {
                var files = Bag.Read(in_stream).ToList();
                using (var out_stream = new MemoryStream())
                {
                    files[0].Save(in_stream, out_stream);
                    var buffer = out_stream.ToArray();
                    var content = Encoding.ASCII.GetString(buffer);

                    Assert.AreEqual(content, "First Test File");
                }
            }
        }

        [Test]
        public void TestExtractMultipleFile()
        {
            using (MemoryStream in_stream = new MemoryStream(ExpectedData.MultipleFiles))
            {
                var expected_content = new string[]
                {
                    "First Test File",
                    "Second Test File"
                };

                var records = Bag.Read(in_stream).ToList();
                for(int i = 0; i < records.Count; ++i)
                {
                    using (var out_stream = new MemoryStream())
                    {
                        records[i].Save(in_stream, out_stream);
                        var buffer = out_stream.ToArray();
                        var content = Encoding.ASCII.GetString(buffer);

                        Assert.AreEqual(content, expected_content[i]);
                    }
                }
            }
        }
    }
}
