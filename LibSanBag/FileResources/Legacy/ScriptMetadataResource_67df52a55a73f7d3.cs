using System.Collections.Generic;
using System.IO;

namespace LibSanBag.FileResources.Legacy
{
    public class ScriptMetadataResource_67df52a55a73f7d3 : ScriptMetadataResourceLegacy
    {
        public override bool IsCompressed => true;

        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var decompressedStream = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                ResourceVersion = decompressedStream.ReadInt32();

                Warnings = ReadString(decompressedStream);

                var unknownA = decompressedStream.ReadInt32(); // 0
                var unknownB = decompressedStream.ReadInt32(); // 0

                var propertiesAreAvailable = decompressedStream.ReadInt32() != 0;
                if (propertiesAreAvailable)
                {
                    var hasEncounteredFirstAttribute = false;
                    var propertyCount = decompressedStream.ReadInt32();
                    Properties = new List<PropertyEntry>(propertyCount);

                    if (propertyCount > 0)
                    {
                        var unknownE = decompressedStream.ReadInt32();
                        for (var propertyIndex = 0; propertyIndex < propertyCount; ++propertyIndex)
                        {
                            var property = ReadProperty(decompressedStream, propertyIndex == 0, ref hasEncounteredFirstAttribute);
                            Properties.Add(property);
                        }
                    }
                }

                var stringsAreAvailable = decompressedStream.ReadInt32() != 0;
                if (stringsAreAvailable)
                {
                    var stringCount = decompressedStream.ReadInt32();
                    Strings = new List<KeyValuePair<string, string>>(stringCount);

                    for (var stringIndex = 0; stringIndex < stringCount; ++stringIndex)
                    {
                        var key = ReadString(decompressedStream);
                        var value = ReadString(decompressedStream);

                        Strings.Add(new KeyValuePair<string, string>(key, value));
                    }
                }
            }
        }
    }
}