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
        private struct TestData
        {
            public string CompressedFilePath { get; set; }
            public string ExpectedAssemblyPath { get; set; }
        }

        private static readonly string RootPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples");
        private static readonly string ExpectedFilePath = Path.Combine(RootPath, "ScriptCompiledBytecode-Resource.dll");

        private byte[] ExpectedBytes { get; set; }
        private IEnumerable<TestData> Tests { get; } = new[]
        {
            new TestData
            {
                CompressedFilePath = Path.Combine(RootPath, "ScriptCompiledBytecode-Resource.bin"),
                ExpectedAssemblyPath = "63d3d75933432b36adca64c6d778a1d7.ScriptSourceText-Resource.v6301a7d31aa6f628.payload.v0.noVariants.dll"
            }
        };

        [SetUp]
        public void Setup()
        {
            ExpectedBytes = File.ReadAllBytes(ExpectedFilePath);
        }

        [Test]
        public void TestConstructCompressedStream()
        {
            foreach (var testData in Tests)
            {
                var compressedFileBytes = File.ReadAllBytes(testData.CompressedFilePath);

                using (var ms = new MemoryStream(compressedFileBytes))
                {
                    var resource = ScriptCompiledBytecodeResource.Create();
                    resource.InitFromStream(ms);
                    Assert.AreEqual(resource.ScriptSourceTextPath, testData.ExpectedAssemblyPath);
                    Assert.AreEqual(resource.AssemblyBytes, ExpectedBytes);
                }
            }
        }

        [Test]
        public void TestConstructFileInfo()
        {
            foreach (var testData in Tests)
            {
                var fileStream = File.OpenRead(testData.CompressedFilePath);
                var fileRecord = new FileRecord
                {
                    Length = (uint)fileStream.Length,
                    Info = null,
                    Offset = 0,
                    TimestampNs = 0,
                    Name = "File Record"
                };

                var resource = ScriptCompiledBytecodeResource.Create();
                resource.InitFromRecord(fileStream, fileRecord);
                Assert.AreEqual(resource.ScriptSourceTextPath, testData.ExpectedAssemblyPath);
                Assert.AreEqual(resource.AssemblyBytes, ExpectedBytes);
            }
        }

        [Test]
        public void TestConstructBytes()
        {
            foreach (var testData in Tests)
            {
                var filebytes = File.ReadAllBytes(testData.CompressedFilePath);

                var resource = ScriptCompiledBytecodeResource.Create();
                resource.InitFromRawCompressed(filebytes);
                Assert.AreEqual(resource.ScriptSourceTextPath, testData.ExpectedAssemblyPath);
                Assert.AreEqual(resource.AssemblyBytes, ExpectedBytes);
            }
        }
    }
}
