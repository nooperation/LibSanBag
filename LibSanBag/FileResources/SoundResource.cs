using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibSanBag.ResourceUtils;

namespace LibSanBag.FileResources
{
    public class SoundResource : BaseFileResource
    {
        public string Name { get; set; }
        public byte[] SoundBytes { get; set; }

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
