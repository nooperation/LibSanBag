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
    class TestScriptSourceTextResource
    {
        byte[] expectedSourceBytes;

        [SetUp]
        public void Setup()
        {
            expectedSourceBytes = File.ReadAllBytes(Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "ScriptSourceText-Resource.cs"));
        }

        [Test]
        public void TestConstructCompressedStream()
        {
            var compressedFileBytes = File.ReadAllBytes(Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "ScriptSourceText-Resource.bin"));

            using (var ms = new MemoryStream(compressedFileBytes))
            {
                var resource = new ScriptSourceTextResource(ms);
                Assert.AreEqual(resource.Filename, "ExampleScript.cs");
                Assert.AreEqual(resource.Source, expectedSourceBytes);
            }
        }

        [Test]
        public void TestConstructFileInfo()
        {
            var fileStream = File.OpenRead(Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "ScriptSourceText-Resource.bin"));
            var fileRecord = new FileRecord
            {
                Length = (uint)fileStream.Length,
                Info = null,
                Offset = 0,
                TimestampNs = 0,
                Name = "File Record"
            };

            var resource = new ScriptSourceTextResource(fileStream, fileRecord);
            Assert.AreEqual(resource.Filename, "ExampleScript.cs");
            Assert.AreEqual(resource.Source, expectedSourceBytes);
        }

        [Test]
        public void TestConstructBytes()
        {
            var filebytes = File.ReadAllBytes(Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "ScriptSourceText-Resource.bin"));

            var resource = new ScriptSourceTextResource(filebytes);
            Assert.AreEqual(resource.Filename, "ExampleScript.cs");
            Assert.AreEqual(resource.Source, expectedSourceBytes);
        }
    }
}
