using LibSanBag.ResourceUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSanBag.FileResources
{
    public class ManifestResource : BaseFileResource
    {
        public class ManifestEntry
        {
            public string HashString { get; set; }
            public long Unknown { get; set; }
            public string Name { get; set; }

            public override string ToString()
            {
                return $"{HashString}.{Name}";
            }
        }

        public List<string> HashList;
        public List<ManifestEntry> Entries;
        public List<Tuple<long, long, long>> UnknownListA;
        public List<int> UnknownListB;

        private static string ReadHash(BinaryReader manifestStream)
        {
            var hashUpper = manifestStream.ReadBytes(0x08);
            var hashLower = manifestStream.ReadBytes(0x08);

            var reverseUpperHash = hashUpper.Reverse().ToArray();
            var reverseLowerHash = hashLower.Reverse().ToArray();

            var hashString = BitConverter.ToString(reverseUpperHash) + BitConverter.ToString(reverseLowerHash);
            hashString = hashString.Replace("-", "").ToLower();

            return hashString;
        }

        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var br = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                var numUnknownHashes = br.ReadInt32();
                HashList = new List<string>(numUnknownHashes);
                for (int i = 0; i < numUnknownHashes; i++)
                {
                    var hashString = ReadHash(br);
                    HashList.Add(hashString);
                }

                var numEntries = br.ReadInt32();
                Entries = new List<ManifestEntry>(numEntries);
                for (int i = 0; i < numEntries; i++)
                {
                    var hashString = ReadHash(br);

                    var unknown = br.ReadInt64();
                    var nameLength = br.ReadInt32();
                    var name = new String(br.ReadChars(nameLength));

                    Entries.Add(new ManifestEntry()
                    {
                        HashString = hashString,
                        Name = name,
                        Unknown = unknown
                    });
                }

                var numUnknownA = br.ReadInt32();
                UnknownListA = new List<Tuple<long, long, long>>(numUnknownA);
                for (int i = 0; i < numUnknownA; i++)
                {
                    UnknownListA.Add(Tuple.Create(
                        br.ReadInt64(),
                        br.ReadInt64(),
                        br.ReadInt64()
                    ));
                }

                var numUnknownB = br.ReadInt32();
                UnknownListB = new List<int>(numUnknownB);
                for (int i = 0; i < numUnknownB; i++)
                {
                    UnknownListB.Add(br.ReadInt32());
                }
            }
        }
    }
}
