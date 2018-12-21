using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LibSanBag.Providers;
using LibSanBag.ResourceUtils;
using LibSanBag.Tests.Providers;
using NUnit.Framework;

namespace LibSanBag.Tests
{
    [TestFixture]
    class TestLibDDS
    {
        public ulong PngHeader => 0x0a1a0a0d474e5089;

        [SetUp]
        public void Setup()
        {
            if (LibDDS.IsAvailable == false)
            {
                Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
                Console.WriteLine("TestLibDds: Setting current directory to " +  Environment.CurrentDirectory);

                LibDDS.FindDependencies(new FileProvider());
            }
        }

        [Test]
        public void TestLibDdsAvailable()
        {
            var fileProvider = new MockFileProvider();
            fileProvider.FileExistsResultQueue.Enqueue(true);

            LibDDS.FindDependencies(fileProvider);

            Assert.IsTrue(LibDDS.IsAvailable);
        }

        [Test]
        public void TestLibDdsNotAvailable()
        {
            var fileProvider = new MockFileProvider();
            fileProvider.FileExistsResultQueue.Enqueue(false);

            LibDDS.FindDependencies(fileProvider);

            Assert.IsFalse(LibDDS.IsAvailable);
        }

        [Test]
        public void TestGetImageBytes()
        {
            var ddsPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Resources", "Texture", "4d0ab27f42b14326ed4987ed25566663.Texture-Resource.v9a8d4bbd19b4cd55.payload.v0.noVariants.dds");
            var ddsBytes = File.ReadAllBytes(ddsPath);

            var imageBytes = LibDDS.GetImageBytesFromDds(ddsBytes, 0, 0, LibDDS.ConversionOptions.CodecType.CODEC_PNG);
            var header = BitConverter.ToUInt64(imageBytes, 0);
            Assert.AreEqual(PngHeader, header);
        }

        [Test]
        public void TestGetImageBytesBadDds()
        {
            var ddsPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Resources", "Texture", "4d0ab27f42b14326ed4987ed25566663.Texture-Resource.v9a8d4bbd19b4cd55.payload.v0.noVariants");
            var ddsBytes = File.ReadAllBytes(ddsPath);

            var ex = Assert.Throws<Exception >(() =>
            {
                LibDDS.GetImageBytesFromDds(ddsBytes, 0, 0, LibDDS.ConversionOptions.CodecType.CODEC_PNG);
            });

            Assert.AreEqual("Failed to read DDS: ERROR LoadFromDDSMemory failed with code: 80004005\n", ex.Message);
        }

        [Test]
        public void TestGetImageBytes_NoLibDDS()
        {
            var ddsPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Resources", "Texture", "4d0ab27f42b14326ed4987ed25566663.Texture-Resource.v9a8d4bbd19b4cd55.payload.v0.noVariants.dds");
            var ddsBytes = File.ReadAllBytes(ddsPath);

            var fileProvider = new MockFileProvider();
            fileProvider.FileExistsResultQueue.Enqueue(false);
            LibDDS.FindDependencies(fileProvider);

            Assert.Throws<Exception>(() =>
            {
                LibDDS.GetImageBytesFromDds(ddsBytes, 0, 0, LibDDS.ConversionOptions.CodecType.CODEC_PNG);
            });
        }
    }
}
