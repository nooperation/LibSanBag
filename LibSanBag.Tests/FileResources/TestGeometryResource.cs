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
        private string CompressedFilePath => Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Resources", "Geometry", "587fcb85b61131545bab31b864ecf6b1.GeometryResource-Resource.v581a503da8d3e98a.payload.v0.noVariants");

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestConstructCompressedStream()
        {
            var compressedFileBytes = File.ReadAllBytes(CompressedFilePath);

            using (var ms = new MemoryStream(compressedFileBytes))
            {
                var resource = GeometryResource.Create();
                resource.InitFromStream(ms);
            }
        }
    }
}
