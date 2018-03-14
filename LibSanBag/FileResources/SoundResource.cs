using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibSanBag.ResourceUtils;

namespace LibSanBag.FileResources
{
    public abstract class SoundResource : BaseFileResource
    {
        public string Name { get; set; }
        public byte[] SoundBytes { get; set; }

        public static SoundResource Create(string version = "")
        {
            switch (version)
            {
                case "8510a121d70371a2":
                case "ffe353a492e99156":
                case "5d4dda35b60493d7":
                default:
                    return new SoundResource_5d4dda35b60493d7();
            }
        }
    }

    public class SoundResource_5d4dda35b60493d7 : SoundResource
    {
        public override bool IsCompressed => true;

        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var decompressedStream = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                var unknown1 = decompressedStream.ReadInt32();
                var unknown2 = decompressedStream.ReadInt32();
                var unknown3 = decompressedStream.ReadInt32();

                var nameLength = decompressedStream.ReadInt32();
                var nameChars = decompressedStream.ReadChars(nameLength);
                Name = new string(nameChars);

                var soundLength = decompressedStream.ReadInt32();
                SoundBytes = decompressedStream.ReadBytes(soundLength);
            }
        }
    }
}
