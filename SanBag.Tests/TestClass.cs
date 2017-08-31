using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanBag.Tests
{
    [TestFixture]
    public class TestClass
    {
        public string OutputPath => Path.Combine(TestContext.CurrentContext.TestDirectory, "out", "Test.bag");

        [Test]
        public void TestEmptyBagCreation()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(OutputPath));

            Bag.CreateNewBag(OutputPath, new List<string>());
            var file_contents = File.ReadAllBytes(OutputPath);
            File.Delete(OutputPath);

            Assert.AreEqual(file_contents, ExpectedData.EmptyBag);
        }

        [Test]
        public void TestSingleFileBagCreation()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(OutputPath));

            Bag.CreateNewBag(OutputPath, new List<string>()
            {
                Path.Combine(TestContext.CurrentContext.TestDirectory, "in", "TestFile1.txt")
            });
            var file_contents = File.ReadAllBytes(OutputPath);
            File.Delete(OutputPath);

            Assert.AreEqual(file_contents, ExpectedData.SingleFile);
        }

        [Test]
        public void TestMultipleFileBagCreation()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(OutputPath));
            Bag.CreateNewBag(OutputPath, new List<string>()
            {
                Path.Combine(TestContext.CurrentContext.TestDirectory, "in", "TestFile1.txt"),
                Path.Combine(TestContext.CurrentContext.TestDirectory, "in", "TestFile2.txt")
            });
            var file_contents = File.ReadAllBytes(OutputPath);
            File.Delete(OutputPath);


            Assert.AreEqual(file_contents, ExpectedData.MultipleFiles);
        }
    }
}
