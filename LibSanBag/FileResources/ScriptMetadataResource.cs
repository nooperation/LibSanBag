using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSanBag.FileResources
{
    public abstract class ScriptMetadataResource : BaseFileResource
    {
        public struct PropertyEntry
        {
            public string Name { get; set; }
            public string Type { get; set; }
        }

        public string Warnings { get; set; }
        public List<PropertyEntry> Properties { get; set; }
        public List<KeyValuePair<string, string>> Strings { get; set; }

        public static ScriptMetadataResource Create(string version = "")
        {
            switch (version)
            {
                case "67df52a55a73f7d3":
                default:
                    return new ScriptMetadataResourceV1();
            }
        }
    }

    public class ScriptMetadataResourceV1 : ScriptMetadataResource
    {
        public override bool IsCompressed => true;

        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var decompressedStream = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                var warningsLength = decompressedStream.ReadInt32();
                Warnings = new string(decompressedStream.ReadChars(warningsLength));

                var unknownA = decompressedStream.ReadInt32(); // 0
                var unknownB = decompressedStream.ReadInt32(); // 0

                var unknownC = decompressedStream.ReadInt32(); // 1
                var propertyCount = decompressedStream.ReadInt32();
                Properties = new List<PropertyEntry>(propertyCount);

                if (propertyCount > 0)
                {
                    var unknownE = decompressedStream.ReadInt32();
                }

                for (var i = 0; i < propertyCount; ++i)
                {
                    var nameLength = decompressedStream.ReadInt32();
                    var name= new string(decompressedStream.ReadChars(nameLength));

                    var typeLength = decompressedStream.ReadInt32();
                    var type = new string(decompressedStream.ReadChars(typeLength));

                    var unknownF = decompressedStream.ReadInt32();
                    var unknownG = decompressedStream.ReadInt32();
                    if (unknownG == 1)
                    {
                        var unknownH = decompressedStream.ReadInt32();
                    }

                    Properties.Add(new PropertyEntry()
                    {
                        Name = name,
                        Type = type
                    });
                }

                var unknownI = decompressedStream.ReadInt32();

                var stringCount = decompressedStream.ReadInt32();
                Strings = new List<KeyValuePair<string, string>>(stringCount);

                for (var i = 0; i < stringCount; ++i)
                {
                    var keyLength = decompressedStream.ReadInt32();
                    var key = new string(decompressedStream.ReadChars(keyLength));

                    var valueLength = decompressedStream.ReadInt32();
                    var value = new string(decompressedStream.ReadChars(valueLength));

                    Strings.Add(new KeyValuePair<string, string>(key, value));
                }
            }
        }
    }
}