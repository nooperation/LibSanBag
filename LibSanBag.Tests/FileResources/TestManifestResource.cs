using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibSanBag.FileResources;
using NUnit.Framework;

namespace LibSanBag.Tests.FileResources
{
    [TestFixture]
    class TestManifestResource
    {

        [Test]
        public void TestMultipleResourceManifest()
        {
            var multipleFileManifest = Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "MultipleFileManifest.bin");
            var manifestBytes = File.ReadAllBytes(multipleFileManifest);
            var manifest = new ManifestResource();
            manifest.InitFromRawDecompressed(manifestBytes);
            CheckMultipleResourceManifest(manifest);
        }

        [Test]
        public void TestMultipleResourceManifestStream()
        {
            var multipleFileManifest = Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "MultipleFileManifest.bin");
            using (var manifestStream = File.OpenRead(multipleFileManifest))
            {
                var manifest = new ManifestResource();
                manifest.InitFromStream(manifestStream);
                CheckMultipleResourceManifest(manifest);
            }
        }

        [Test]
        public void TestMultipleResourceManifestFileRecord()
        {
            var multipleFileManifest = Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "MultipleFileManifest.bin");
            using (var manifestStream = File.OpenRead(multipleFileManifest))
            {
                var fileRecord = new FileRecord
                {
                    Length = (uint)manifestStream.Length,
                    Info = null,
                    Offset = 0,
                    TimestampNs = 0,
                    Name = "File Record"
                };

                var manifest = new ManifestResource();
                manifest.InitFromRecord(manifestStream, fileRecord);
                CheckMultipleResourceManifest(manifest);
            }
        }

        [Test]
        public void TestMultipleHashManifest()
        {
            var multipleHashManifest = Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "MultipleHashManifest.bin");
            var manifestBytes = File.ReadAllBytes(multipleHashManifest);
            var manifest = new ManifestResource();

            manifest.InitFromRawDecompressed(manifestBytes);
            CheckMultipleHashManifest(manifest);
        }

        [Test]
        public void TestMultipleHashManifestStream()
        {
            var multipleHashManifest = Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "MultipleHashmanifest.bin");
            using (var manifestStream = File.OpenRead(multipleHashManifest))
            {
                var manifest = new ManifestResource();
                manifest.InitFromStream(manifestStream);
                CheckMultipleHashManifest(manifest);
            }
        }

        [Test]
        public void TestMultipleHashManifestFileRecord()
        {
            var multipleHashManifest = Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "MultipleHashManifest.bin");
            using (var manifestStream = File.OpenRead(multipleHashManifest))
            {
                var fileRecord = new FileRecord
                {
                    Length = (uint)manifestStream.Length,
                    Info = null,
                    Offset = 0,
                    TimestampNs = 0,
                    Name = "File Record"
                };

                var manifest = new ManifestResource();
                manifest.InitFromRecord(manifestStream, fileRecord);
                CheckMultipleHashManifest(manifest);
            }
        }

        private void CheckMultipleResourceManifest(ManifestResource manifest)
        {
            var expectedEntries = new List<ManifestResource.ManifestEntry>()
            {
                new ManifestResource.ManifestEntry
                {
                    Name = "File-Resource",
                    HashString = "d3f57263f4e629f9c1ec1ca3c25ccfae",
                    Unknown = 0,
                },
                new ManifestResource.ManifestEntry
                {
                    Name = "Texture-Import",
                    HashString = "4d0ab27f42b14326ed4987ed25566663",
                    Unknown = 0,
                },
            };
            var expectedHashes = new List<string>()
            {
            };
            var expectedUnknownA = new List<Tuple<long, long, long>>()
            {
                new Tuple<long, long, long>(0, 1, 1),
                new Tuple<long, long, long>(-1, -1, 2),
            };
            var expectedUnknownB = new List<int>()
            {
                1,
            };

            Assert.AreEqual(expectedEntries.Count, manifest.Entries.Count);
            for (var i = 0; i < manifest.Entries.Count; i++)
            {
                Assert.AreEqual(expectedEntries[i].HashString, manifest.Entries[i].HashString);
                Assert.AreEqual(expectedEntries[i].Name, manifest.Entries[i].Name);
                Assert.AreEqual(expectedEntries[i].Unknown, manifest.Entries[i].Unknown);
            }
            Assert.True(expectedHashes.SequenceEqual(manifest.HashList));
            Assert.True(expectedUnknownA.SequenceEqual(manifest.UnknownListA));
            Assert.True(expectedUnknownB.SequenceEqual(manifest.UnknownListB));
        }

        private void CheckMultipleHashManifest(ManifestResource manifest)
        {
            var expectedEntries = new List<ManifestResource.ManifestEntry>()
            {
                new ManifestResource.ManifestEntry
                {
                    Name = "LuaScript-Resource",
                    HashString = "af86c2b4266db473d57fe6b08f609ee6",
                    Unknown = 0,
                },
            };
            var expectedHashes = new List<string>()
            {
                "7f709ed3b97b2cc76a2ec6d49bda6ab3",
                "2578df524038e7b266d33cb46f6a4b41",
                "31620f0df57b26bbee566c37b78f5be4",
                "7ada26eeb44f015a855bd7d1c1ca1f10",
                "f88cd480b0d40a83c9312319f47fbb73",
                "ed3f2ae137f6a7dd0a436eefbfab2334",
                "b87690a2355c318029249055318d06b9",
                "b4b27173e54350c1f0b5ac9136fa464e",
                "f89cf7069bef5a24a84ade56e58e5171",
                "f3fbb248f0add27fa441dbaf60fe9a71",
                "eda1ef90b00a6a8562025d147998474f",
                "7d654d2280096e160c2535bb869e3acf",
                "0d366eab88a16c2f824738afb29098ad",
                "f1b2d12d05678ef7b5fb62d8cc4e9b3d",
            };
            var expectedUnknownA = new List<Tuple<long, long, long>>()
            {
                new Tuple<long, long, long>(-1, -1, 1),
            };
            var expectedUnknownB = new List<int>()
            {
            };

            Assert.AreEqual(expectedEntries.Count, manifest.Entries.Count);
            for (var i = 0; i < manifest.Entries.Count; i++)
            {
                Assert.AreEqual(expectedEntries[i].HashString, manifest.Entries[i].HashString);
                Assert.AreEqual(expectedEntries[i].Name, manifest.Entries[i].Name);
                Assert.AreEqual(expectedEntries[i].Unknown, manifest.Entries[i].Unknown);
            }
            Assert.True(expectedHashes.SequenceEqual(manifest.HashList));
            Assert.True(expectedUnknownA.SequenceEqual(manifest.UnknownListA));
            Assert.True(expectedUnknownB.SequenceEqual(manifest.UnknownListB));
        }

        [Test]
        public void TestManifestToString()
        {
            var multipleHashManifest = Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "MultipleHashManifest.bin");
            var manifestBytes = File.ReadAllBytes(multipleHashManifest);
            var manifest = new ManifestResource();
            manifest.InitFromRawDecompressed(manifestBytes);

            var name = manifest.Entries[0].ToString();
            var expectedName = "af86c2b4266db473d57fe6b08f609ee6.LuaScript-Resource";

            Assert.AreEqual(expectedName, name);
        }

        [Test]
        public void TestOneHashOneFile()
        {
            var manifestPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "ManifestOneHashOneFile.bin");
            var manifestBytes = File.ReadAllBytes(manifestPath);
            var manifest = new ManifestResource();
            manifest.InitFromRawDecompressed(manifestBytes);

            var name = manifest.Entries[0].ToString();
            var expectedName = "f6cb837a26516e3693f255a9cdc63d9a.ScriptCompiledBytecode-Resource";

            Assert.AreEqual(expectedName, name);
        }
    }
}
