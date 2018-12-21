using System.Collections.Generic;
using System.IO;
using LibSanBag.FileResources;
using NUnit.Framework;

namespace LibSanBag.Tests.FileResources
{
    [TestFixture]
    internal class TestGeometryResourceCanonical : BaseFileResourceTest
    {
        private struct TestData
        {
            public string CompressedFilePath { get; }
            public byte[] ExpectedContentBytes { get; }
            public FileRecordInfo RecordInfo { get; }

            public TestData(string compressedFilePath, string expectedContentPath)
            {
                CompressedFilePath = Path.Combine(RootPath, compressedFilePath);
                ExpectedContentBytes = File.ReadAllBytes(Path.Combine(RootPath, expectedContentPath));
                RecordInfo = FileRecordInfo.Create(compressedFilePath);
            }
        }

        private static readonly string RootPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Resources", "Geometry");
        private IEnumerable<TestData> Tests { get; } = new[]
        {
            new TestData("a1142263356f82516bd7908acf4527cd.GeometryResource-Canonical.v51b89e39caab7b79.payload.v0.noVariants", "BasicCube.fbx"),
        };

        [Test]
        public void TestConstructCompressedStream()
        {
            foreach (var testData in Tests)
            {
                var compressedFileBytes = File.ReadAllBytes(testData.CompressedFilePath);

                using (var ms = new MemoryStream(compressedFileBytes))
                {
                    var resource = GeometryResourceCanonical.Create(testData.RecordInfo.VersionHash);
                    resource.InitFromStream(ms);

                    Assert.AreEqual(resource.Content, testData.ExpectedContentBytes);
                }
            }
        }
    }
}
