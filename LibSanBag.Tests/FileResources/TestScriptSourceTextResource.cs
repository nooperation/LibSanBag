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
        private string expectedSource;

        private string CompressedFilePath => Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Resources", "ScriptSourceText", "4b6e1436d155d91deeab038bb225ab21.ScriptSourceText-Resource.v4cde67396803610f.payload.v0.noVariants");
        private string ExpectedFilePath => Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Resources", "ScriptSourceText", "ScriptSourceText-Resource.cs");
        private string ExpectedSourceTextFilename => ""; //"ExampleScript.cs";

        [SetUp]
        public void Setup()
        {
            expectedSource = File.ReadAllText(ExpectedFilePath);
        }

        [Test]
        public void TestConstructCompressedStream()
        {
            var compressedFileBytes = File.ReadAllBytes(CompressedFilePath);

            using (var ms = new MemoryStream(compressedFileBytes))
            {
                var resource = ScriptSourceTextResource.Create("4cde67396803610f");
                resource.InitFromStream(ms);
                Assert.AreEqual(resource.Filename, ExpectedSourceTextFilename);
                Assert.AreEqual(resource.Source, expectedSource);
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

            var resource = ScriptSourceTextResource.Create("4cde67396803610f");
            resource.InitFromRecord(fileStream, fileRecord);
            Assert.AreEqual(resource.Filename, ExpectedSourceTextFilename);
            Assert.AreEqual(resource.Source, expectedSource);
        }

        [Test]
        public void TestConstructBytes()
        {
            var filebytes = File.ReadAllBytes(CompressedFilePath);

            var resource = ScriptSourceTextResource.Create("4cde67396803610f");
            resource.InitFromRawCompressed(filebytes);
            Assert.AreEqual(resource.Filename, ExpectedSourceTextFilename);
            Assert.AreEqual(resource.Source, expectedSource);
        }
    }
}
