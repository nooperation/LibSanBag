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
        byte[] expectedAssemblyBytes;

        [SetUp]
        public void Setup()
        {
            expectedAssemblyBytes = File.ReadAllBytes(Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "ScriptCompiledBytecode-Resource.dll"));
        }

        //[Test]
        public void TestConstructCompressedStream()
        {
            var compressedFileBytes = File.ReadAllBytes(Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "ScriptCompiledBytecode-Resource.bin"));

            using (var ms = new MemoryStream(compressedFileBytes))
            {
                var resource = new ScriptCompiledBytecodeResource(ms);
                Assert.AreEqual(resource.ScriptSourceTextPath, "63d3d75933432b36adca64c6d778a1d7.ScriptSourceText-Resource.v6301a7d31aa6f628.payload.v0.noVariants.dll");
                Assert.AreEqual(resource.AssemblyBytes, expectedAssemblyBytes);
            }
        }

        //[Test]
        public void TestConstructFileInfo()
        {
            var fileStream = File.OpenRead(Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "ScriptCompiledBytecode-Resource.bin"));
            var fileRecord = new FileRecord
            {
                Length = (uint)fileStream.Length,
                Info = null,
                Offset = 0,
                TimestampNs = 0,
                Name = "File Record"
            };

            var resource = new ScriptCompiledBytecodeResource(fileStream, fileRecord);
            Assert.AreEqual(resource.ScriptSourceTextPath, "63d3d75933432b36adca64c6d778a1d7.ScriptSourceText-Resource.v6301a7d31aa6f628.payload.v0.noVariants.dll");
            Assert.AreEqual(resource.AssemblyBytes, expectedAssemblyBytes);
        }

       // [Test]
        public void TestConstructBytes()
        {
            var filebytes = File.ReadAllBytes(Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "ScriptCompiledBytecode-Resource.bin"));

            var resource = new ScriptCompiledBytecodeResource(filebytes);
            Assert.AreEqual(resource.ScriptSourceTextPath, "63d3d75933432b36adca64c6d778a1d7.ScriptSourceText-Resource.v6301a7d31aa6f628.payload.v0.noVariants.dll");
            Assert.AreEqual(resource.AssemblyBytes, expectedAssemblyBytes);
        }
    }
}
