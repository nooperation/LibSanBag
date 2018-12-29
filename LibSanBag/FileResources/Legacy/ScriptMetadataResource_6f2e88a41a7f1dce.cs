using System.Collections.Generic;
using System.IO;

namespace LibSanBag.FileResources.Legacy
{
    public class ScriptMetadataResource_6f2e88a41a7f1dce : ScriptMetadataResourceLegacy
    {
        public override bool IsCompressed => true;

        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            var unknownE = 0;

            using (var decompressedStream = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                Warnings = ReadString(decompressedStream);

                var unknownA = decompressedStream.ReadInt32(); // 0
                var unknownB = decompressedStream.ReadInt32(); // 0

                var unknownC = decompressedStream.ReadInt32();
                var unknownD = 0;
                if (unknownC > 0)
                {
                    unknownD = decompressedStream.ReadInt32();
                    if (unknownD > 0)
                    {
                        unknownE = decompressedStream.ReadInt32();
                        AssemblyName = ReadString(decompressedStream);

                        if(unknownE > 1)
                        {
                            var unknown_usually_2 = decompressedStream.ReadInt32();
                        }
                    }
                }

                var propertiesAreAvailable = decompressedStream.ReadInt32();
                if (propertiesAreAvailable > 0)
                {
                    var hasEncounteredFirstAttribute = false;
                    var propertyCount = decompressedStream.ReadInt32();
                    Properties = new List<PropertyEntry>(propertyCount);

                    if (propertyCount > 0)
                    {
                        var unknownF = decompressedStream.ReadInt32();
                        for (var propertyIndex = 0; propertyIndex < propertyCount; ++propertyIndex)
                        {
                            var property = ReadProperty(decompressedStream, propertyIndex == 0, ref hasEncounteredFirstAttribute);
                            Properties.Add(property);
                        }
                    }
                }

                if (unknownD > 0)
                {
                    var assemblyNameAgain = ReadString(decompressedStream);
                    if(unknownE == 3)
                    {
                        var assemblyNameAgain2 = ReadString(decompressedStream);
                        var assemblyNameAgain3 = ReadString(decompressedStream);
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