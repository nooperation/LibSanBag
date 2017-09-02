using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSanBag
{
    public class Manifest
    {
        public long NextManifestOffset { get; set; } = 0;
        public int NextManifestLength { get; set; } = 0;
        public List<FileRecord> Records { get; set; } = new List<FileRecord>();

        public void Read(BinaryReader inStream, long offset, int length)
        {
            inStream.BaseStream.Seek(offset, SeekOrigin.Begin);
            NextManifestOffset = inStream.ReadInt64();
            NextManifestLength = inStream.ReadInt32();
            Records = new List<FileRecord>();

            while (inStream.BaseStream.Position < offset + length)
            {
                var recordMarker = inStream.ReadByte();
                if (recordMarker != 0xFF)
                {
                    break;
                }

                var newRecord = new FileRecord(inStream);
                Records.Add(newRecord);
            }
        }
    }
}
