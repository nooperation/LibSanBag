using LibSanBag.FileResources;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSanBag.Tests.FileResources
{
    [TestFixture]
    class TestScriptCompiledBytecodeResource
    {
        private byte[] expectedAssemblyBytes;

        private string CompressedFilePath => Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "ScriptCompiledBytecode-Resource.bin");
        private string ExpectedFilePath => Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "ScriptCompiledBytecode-Resource.dll");
        private string ExpectedScriptSourceTextPath => "63d3d75933432b36adca64c6d778a1d7.ScriptSourceText-Resource.v6301a7d31aa6f628.payload.v0.noVariants.dll";

        [SetUp]
        public void Setup()
        {
            expectedAssemblyBytes = File.ReadAllBytes(ExpectedFilePath);
        }

        [Test]
        public void TestConstructCompressedStream()
        {
            var compressedFileBytes = File.ReadAllBytes(CompressedFilePath);

            using (var ms = new MemoryStream(compressedFileBytes))
            {
                var resource = new ScriptCompiledBytecodeResource();
                resource.InitFromStream(ms);
                Assert.AreEqual(resource.ScriptSourceTextPath, ExpectedScriptSourceTextPath);
                Assert.AreEqual(resource.AssemblyBytes, expectedAssemblyBytes);
            }
        }

        [Test]
        public void TestConstructFileInfo()
        {
            var fileStream = File.OpenRead(CompressedFilePath);
            var fileRecord = new FileRecord
            {
                Length = (uint)fileStream.Length,
                Info = null,
                Offset = 0,
                TimestampNs = 0,
                Name = "File Record"
            };

            var resource = new ScriptCompiledBytecodeResource();
            resource.InitFromRecord(fileStream, fileRecord);
            Assert.AreEqual(resource.ScriptSourceTextPath, ExpectedScriptSourceTextPath);
            Assert.AreEqual(resource.AssemblyBytes, expectedAssemblyBytes);
        }

        [Test]
        public void TestConstructBytes()
        {
            var filebytes = File.ReadAllBytes(CompressedFilePath);

            var resource = new ScriptCompiledBytecodeResource();
            resource.InitFromRawCompressed(filebytes);
            Assert.AreEqual(resource.ScriptSourceTextPath, ExpectedScriptSourceTextPath);
            Assert.AreEqual(resource.AssemblyBytes, expectedAssemblyBytes);
        }
    }
}
