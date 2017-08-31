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
        private string OutputPath => Path.Combine(TestContext.CurrentContext.TestDirectory, "out", TestContext.CurrentContext.Test.Name + ".bag");
        private Mock<ITimeProvider> MockTimeProvider = new Mock<ITimeProvider>();

        [SetUp]
        public void Setup()
        {
            MockTimeProvider.Setup(func => func.GetCurrentTime()).Returns(0x14DF773F94417DC0);
        }

        [Test]
        public void TestEmptyBagCreation()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(OutputPath));

            Bag.Write(OutputPath, new List<string>(), MockTimeProvider.Object);
            var file_contents = File.ReadAllBytes(OutputPath);
            File.Delete(OutputPath);

            Assert.AreEqual(file_contents, ExpectedData.EmptyBag);
        }

        [Test]
        public void TestSingleFileBagCreation()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(OutputPath));

            Bag.Write(OutputPath, new List<string>()
            {
                Path.Combine(TestContext.CurrentContext.TestDirectory, "in", "TestFile1.txt")
            }, MockTimeProvider.Object);
            var file_contents = File.ReadAllBytes(OutputPath);
            File.Delete(OutputPath);

            Assert.AreEqual(file_contents, ExpectedData.SingleFile);
        }

        [Test]
        public void TestMultipleFileBagCreation()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(OutputPath));
            Bag.Write(OutputPath, new List<string>()
            {
                Path.Combine(TestContext.CurrentContext.TestDirectory, "in", "TestFile1.txt"),
                Path.Combine(TestContext.CurrentContext.TestDirectory, "in", "TestFile2.txt")
            }, MockTimeProvider.Object);
            var file_contents = File.ReadAllBytes(OutputPath);
            File.Delete(OutputPath);


            Assert.AreEqual(file_contents, ExpectedData.MultipleFiles);
        }
    }
}
