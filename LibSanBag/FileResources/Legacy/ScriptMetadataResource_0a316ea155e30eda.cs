using System.Collections.Generic;
using System.IO;

namespace LibSanBag.FileResources.Legacy
{
    public class ScriptMetadataResource_0a316ea155e30eda : ScriptMetadataResourceLegacy
    {
        public override bool IsCompressed => true;

        public PropertyEntry ReadProperty(BinaryReader decompressedStream)
        {
            var name = ReadString(decompressedStream);
            var type = ReadString(decompressedStream);

            return new PropertyEntry()
            {
                Attributes = new List<KeyValuePair<string, string>>(),
                Name = name,
                Type = type
            };
        }

        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var decompressedStream = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                ResourceVersion = decompressedStream.ReadInt32();

                Warnings = "";
                Strings = new List<KeyValuePair<string, string>>();

                var propertyCount = decompressedStream.ReadInt32();
                Properties = new List<PropertyEntry>(propertyCount);

                if (propertyCount > 0)
                {
                    for (var propertyIndex = 0; propertyIndex < propertyCount; ++propertyIndex)
                    {
                        var property = ReadProperty(decompressedStream);
                        Properties.Add(property);
                    }
                }
            }
        }
    }
}