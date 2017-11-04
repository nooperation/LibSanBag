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


        [Test]
        public void TestSetup_InternalException()
        {
            var fileProvider = new MockFileProvider();
            var registryProvider = new MockRegistryProvider();
            var environmentProvider = new MockEnvironmentProvider();

            OodleLz.SetupEnvironment(fileProvider, environmentProvider, registryProvider);

            Assert.IsFalse(OodleLz.IsAvailable);
        }

        [Test]
        public void TestSetup_YesLocalFile()
        {
            var fileProvider = new MockFileProvider();
            fileProvider.FileExistsResultQueue.Enqueue(true);
            
            var registryProvider = new MockRegistryProvider();
            var environmentProvider = new MockEnvironmentProvider();

            OodleLz.SetupEnvironment(fileProvider, environmentProvider, registryProvider);

            Assert.IsTrue(OodleLz.IsAvailable);
        }

        [Test]
        public void TestSetup_NoLocalFile_NoRegistry()
        {
            var fileProvider = new MockFileProvider();
            fileProvider.FileExistsResultQueue.Enqueue(false);

            var registryProvider = new MockRegistryProvider();
            registryProvider.ReturnValueQueue.Enqueue(null);
            registryProvider.ReturnValueQueue.Enqueue(null);

            var environmentProvider = new MockEnvironmentProvider();

            OodleLz.SetupEnvironment(fileProvider, environmentProvider, registryProvider);

            Assert.IsFalse(OodleLz.IsAvailable);
        }

        [Test]
        public void TestSetup_NoLocalFile_YesRegistry_NoRegistryFile()
        {
            var fileProvider = new MockFileProvider();
            fileProvider.FileExistsResultQueue.Enqueue(false);
            fileProvider.FileExistsResultQueue.Enqueue(false);

            var registryProvider = new MockRegistryProvider();
            registryProvider.ReturnValueQueue.Enqueue(@"c:\program files\sansar");

            var environmentProvider = new MockEnvironmentProvider();

            OodleLz.SetupEnvironment(fileProvider, environmentProvider, registryProvider);

            Assert.IsFalse(OodleLz.IsAvailable);
        }

        [Test]
        public void TestSetup_NoLocalFile_YesRegistry_YesRegistryFile()
        {
            var sansarDirectory = @"c:\program files\sansar";
            var systemPath = "current_path";

            var fileProvider = new MockFileProvider();
            fileProvider.FileExistsResultQueue.Enqueue(false);
            fileProvider.FileExistsResultQueue.Enqueue(true);

            var registryProvider = new MockRegistryProvider();
            registryProvider.ReturnValueQueue.Enqueue(sansarDirectory);

            var environmentProvider = new MockEnvironmentProvider();
            environmentProvider.EnvironmentVariableQueue.Enqueue(systemPath);

            OodleLz.SetupEnvironment(fileProvider, environmentProvider, registryProvider);

            Assert.AreEqual("PATH", environmentProvider.LastSetVariable);
            Assert.AreEqual($"{systemPath};{sansarDirectory}\\Client", environmentProvider.LastSetValue);
            Assert.IsTrue(OodleLz.IsAvailable);
        }

        [Test]
        public void TestExtractWhileUnavailable()
        {
            var fileProvider = new MockFileProvider();
            fileProvider.FileExistsResultQueue.Enqueue(false);

            var registryProvider = new MockRegistryProvider();
            registryProvider.ReturnValueQueue.Enqueue(null);
            registryProvider.ReturnValueQueue.Enqueue(null);

            var environmentProvider = new MockEnvironmentProvider();

            OodleLz.SetupEnvironment(fileProvider, environmentProvider, registryProvider);

            var path = Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "OodleLz", "Compressed_F2_Resource.bin");
            Assert.Throws<Exception>(() =>
            {
                OodleLz.DecompressResource(path);
            });
        }
    }
}
