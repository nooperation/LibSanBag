using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibSanBag.ResourceUtils;
using NUnit.Framework;

namespace LibSanBag.Tests
{
    [TestFixture]
    class TestLibFSB
    {
        [Test]
        public void TestGetImageBytes()
        {
            var fsbPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Sample.fsb");
            var fsbBytes = File.ReadAllBytes(fsbPath);

            var expectedWavPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Sample.wav");
            var expectedWavBytes = File.ReadAllBytes(expectedWavPath);

            var outputWavPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "output.wav");
            LibFSB.SaveAs(fsbBytes, outputWavPath);

            var outputWavBytes = File.ReadAllBytes(outputWavPath);
            Assert.AreEqual(expectedWavBytes, outputWavBytes);
        }

    }
}
