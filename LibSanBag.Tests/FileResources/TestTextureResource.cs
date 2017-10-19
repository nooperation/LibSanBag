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
    class TestTextureResource
    {
        byte[] expectedTextureBytes;

        [SetUp]
        public void Setup()
        {
            expectedTextureBytes = File.ReadAllBytes(Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Texture-Resource.dds"));
        }

        [Test]
        public void TestConstructCompressedStream()
        {
            var files = Directory.GetFiles(Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples"), "*.*", SearchOption.TopDirectoryOnly);
            Console.WriteLine("Files in samples directory:");
            foreach (var file in files)
            {
                Console.WriteLine("  " + file);
            }

            byte[] compressedFileBytes = null;
            try
            {
                compressedFileBytes = File.ReadAllBytes(Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Texture-Resource.bin"));
                Console.WriteLine("Bytes read: " + compressedFileBytes.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to read all bytes -> " + ex.Message);
            }


            try
            {
                using (var ms = new MemoryStream(compressedFileBytes))
                {
                    var resource = new TextureResource(ms);
                    Assert.AreEqual(resource.DdsBytes, expectedTextureBytes);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Test failed because ===> " + ex.Message);
                if (compressedFileBytes == null)
                    Console.WriteLine("compressedFileBytes is null");
                else
                    Console.WriteLine("compressedFileBytes is NOT null");

                throw;
            }

        }

        //[Test]
        public void TestConstructFileInfo()
        {
            var fileStream = File.OpenRead(Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Texture-Resource.bin"));
            var fileRecord = new FileRecord
            {
                Length = (uint)fileStream.Length,
                Info = null,
                Offset = 0,
                TimestampNs = 0,
                Name = "File Record"
            };

            var resource = new TextureResource(fileStream, fileRecord);
            Assert.AreEqual(resource.DdsBytes, expectedTextureBytes);
        }

        //[Test]
        public void TestConstructBytes()
        {
            var filebytes = File.ReadAllBytes(Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Texture-Resource.bin"));

            var resource = new TextureResource(filebytes);
            Assert.AreEqual(resource.DdsBytes, expectedTextureBytes);
        }
    }
}
