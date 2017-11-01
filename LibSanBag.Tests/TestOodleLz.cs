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
    class TestOodleLz
    {
        public readonly List<Tuple<string, string>> CompressedDataSamples = new List<Tuple<string, string>>
        {
            new Tuple<string, string>
            (
                Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "OodleLz", "Compressed_01_Resource.bin"),
                Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "OodleLz", "Decompressed_01_Resource.bin")
            ),
            new Tuple<string, string>
            (
                Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "OodleLz", "Compressed_F1_Resource.bin"),
                Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "OodleLz", "Decompressed_F1_Resource.bin")
            ),
            new Tuple<string, string>
            (
                Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "OodleLz", "Compressed_F2_Resource.bin"),
                Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "OodleLz", "Decompressed_F2_Resource.bin")
            ),
        };

        [Test]
        public void TestDecompressResource_String()
        {
            foreach (var sample in CompressedDataSamples)
            {
                var decompressedBytes = OodleLz.DecompressResource(sample.Item1);
                var expectedBytes = File.ReadAllBytes(sample.Item2);
                Assert.AreEqual(decompressedBytes, expectedBytes);
            }
        }
        [Test]
        public void TestBadOodleHeader()
        {
            var path = Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "OodleLz", "BadOodleHeader.bin");

            Assert.Throws<Exception>(() =>
            {
                OodleLz.DecompressResource(path);
            });
        }

        [Test]
        public void TestUnknownCompressionHeader()
        {
            var path = Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "OodleLz", "UnknownCompressionHeader.bin");

            Assert.Throws<Exception>(() =>
            {
                OodleLz.DecompressResource(path);
            });
        }
    }
}
