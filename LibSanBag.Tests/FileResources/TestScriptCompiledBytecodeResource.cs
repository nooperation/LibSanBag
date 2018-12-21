using LibSanBag.FileResources;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

namespace LibSanBag.Tests.FileResources
{
    [TestFixture]
    internal class TestScriptCompiledBytecodeResource : BaseFileResourceTest
    {
        private struct TestData
        {
            public string CompressedFilePath { get; }
            public string AssemblyPath { get; }
            public string ExpectedAssemblyPath { get; }
            public FileRecordInfo RecordInfo { get; }

            public TestData(string compressedFilePath, string assemblyPath, string expectedAssemblyPath)
            {
                CompressedFilePath = Path.Combine(RootPath, compressedFilePath);
                AssemblyPath = Path.Combine(RootPath, assemblyPath);
                ExpectedAssemblyPath = expectedAssemblyPath;
                RecordInfo = FileRecordInfo.Create(compressedFilePath);
            }
        }

        private static readonly string RootPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Resources", "ScriptCompiledBytecode");

        private IEnumerable<TestData> Tests { get; } = new[]
        {
            new TestData(
                "63d3d75933432b36adca64c6d778a1d7.ScriptCompiledBytecode-Resource.vc84707da067146a9.payload.v0.noVariants",
                "63d3d75933432b36adca64c6d778a1d7.ScriptSourceText-Resource.v6301a7d31aa6f628.payload.v0.noVariants.dll",
                "63d3d75933432b36adca64c6d778a1d7.ScriptSourceText-Resource.v6301a7d31aa6f628.payload.v0.noVariants.dll"
            ),
            new TestData(
                "4b6e1436d155d91deeab038bb225ab21.ScriptCompiledBytecode-Resource.v695aad7e1181dc46.payload.v0.noVariants",
                "4b6e1436d155d91deeab038bb225ab21.ScriptSourceText-Resource.v4cde67396803610f.payload.v0.noVariants.dll",
                string.Empty
            ),
        };

        [Test]
        public void TestConstructCompressedStream()
        {
            foreach (var testData in Tests)
            {
                var compressedFileBytes = File.ReadAllBytes(testData.CompressedFilePath);

                using (var ms = new MemoryStream(compressedFileBytes))
                {
                    var resource = ScriptCompiledBytecodeResource.Create(testData.RecordInfo.VersionHash);
                    resource.InitFromStream(ms);

                    var expectedAssemblyBytes = File.ReadAllBytes(testData.AssemblyPath);
                    Assert.AreEqual(resource.ScriptSourceTextPath, testData.ExpectedAssemblyPath);
                    Assert.AreEqual(resource.AssemblyBytes, expectedAssemblyBytes);
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

                var resource = ScriptCompiledBytecodeResource.Create(testData.RecordInfo.VersionHash);
                resource.InitFromRecord(fileStream, fileRecord);

                var expectedAssemblyBytes = File.ReadAllBytes(testData.AssemblyPath);
                Assert.AreEqual(resource.ScriptSourceTextPath, testData.ExpectedAssemblyPath);
                Assert.AreEqual(resource.AssemblyBytes, expectedAssemblyBytes);
            }
        }

        [Test]
        public void TestConstructBytes()
        {
            foreach (var testData in Tests)
            {
                var filebytes = File.ReadAllBytes(testData.CompressedFilePath);

                var resource = ScriptCompiledBytecodeResource.Create(testData.RecordInfo.VersionHash);
                resource.InitFromRawCompressed(filebytes);

                var expectedAssemblyBytes = File.ReadAllBytes(testData.AssemblyPath);
                Assert.AreEqual(resource.ScriptSourceTextPath, testData.ExpectedAssemblyPath);
                Assert.AreEqual(resource.AssemblyBytes, expectedAssemblyBytes);
            }
        }
    }
}
