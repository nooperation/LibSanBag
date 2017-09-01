using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanBag
{
    public class Manifest
    {
        public long NextManifestOffset { get; set; } = 0;
        public int NextManifestLength { get; set; } = 0;
        public List<FileRecord> Records { get; set; } = new List<FileRecord>();

        public void Read(BinaryReader in_stream, long offset, int length)
        {
            in_stream.BaseStream.Seek(offset, SeekOrigin.Begin);
            NextManifestOffset = in_stream.ReadInt64();
            NextManifestLength = in_stream.ReadInt32();
            Records = new List<FileRecord>();

            while (in_stream.BaseStream.Position < offset + length)
            {
                var record_marker = in_stream.ReadByte();
                if (record_marker != 0xFF)
                {
                    break;
                }

                var new_record = new FileRecord(in_stream);
                Records.Add(new_record);
            }
        }
    }
}
