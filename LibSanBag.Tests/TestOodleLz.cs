using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibSanBag.Providers;
using LibSanBag.ResourceUtils;
using LibSanBag.Tests.Providers;
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

        [SetUp]
        public void Setup()
        {
            Console.WriteLine("TestOoodleLz: Setup()");
            Console.WriteLine("Current directory: " + Environment.CurrentDirectory);
            Console.WriteLine("------Start----------");
            foreach (var file in Directory.GetFiles(Environment.CurrentDirectory))
            {
                Console.WriteLine("  -> " + file);
            }
            Console.WriteLine("------End----------");

            if (Unpacker.IsAvailable == false)
            {
                Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
                Console.WriteLine("TestOoodleLz: Setting current directory to " +  Environment.CurrentDirectory);

                Unpacker.FindDependencies(new FileProvider());

                Console.WriteLine("TestOoodleLz: Available = " + Unpacker.IsAvailable);
            }
        }

        [Test]
        public void TestDecompressResource_String()
        {
            foreach (var sample in CompressedDataSamples)
            {
                var decompressedBytes = Unpacker.DecompressResource(sample.Item1);
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
                Unpacker.DecompressResource(path);
            });
        }

        [Test]
        public void TestUnknownCompressionHeader()
        {
            var path = Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "OodleLz", "UnknownCompressionHeader.bin");

            Assert.Throws<NotImplementedException>(() =>
            {
                Unpacker.DecompressResource(path);
            });
        }

        [Test]
        public void TestExtractWhileUnavailable()
        {
            var fileProvider = new MockFileProvider();
            fileProvider.FileExistsResultQueue.Enqueue(false);

            Unpacker.FindDependencies(fileProvider);

            var path = Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "OodleLz", "Compressed_F2_Resource.bin");
            Assert.Throws<Exception>(() =>
            {
                Unpacker.DecompressResource(path);
            });
        }
    }
}
