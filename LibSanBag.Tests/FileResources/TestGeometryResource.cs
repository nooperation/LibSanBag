using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibSanBag.FileResources;
using NUnit.Framework;

namespace LibSanBag.Tests.FileResources
{
    [TestFixture]
    class TestGeometryResource
    {
        private struct TestData
        {
            public string CompressedFilePath { get; set; }
            public FileRecordInfo RecordInfo { get; set; }

            public TestData(string compressedFilePath)
            {
                CompressedFilePath = Path.Combine(RootPath, compressedFilePath);
                RecordInfo = FileRecordInfo.Create(compressedFilePath);
            }
        }

        private static readonly string RootPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Resources", "Geometry");

        private IEnumerable<TestData> Tests { get; } = new[]
        {
            new TestData("587fcb85b61131545bab31b864ecf6b1.GeometryResource-Resource.v581a503da8d3e98a.payload.v0.noVariants"),
        };

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestConstructCompressedStream()
        {
            foreach (var testData in Tests)
            {
                var compressedFileBytes = File.ReadAllBytes(testData.CompressedFilePath);

                using (var ms = new MemoryStream(compressedFileBytes))
                {
                    var resource = GeometryResource.Create(testData.RecordInfo.VersionHash);
                    resource.InitFromStream(ms);
                }
            }
        }
    }
}
