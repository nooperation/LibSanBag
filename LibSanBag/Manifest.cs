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
        /// <summary>
        /// File offset to the next bag manifest
        /// </summary>
        public long NextManifestOffset { get; set; } = 0;
        /// <summary>
        /// Length of the next manifest
        /// </summary>
        public int NextManifestLength { get; set; } = 0;
        /// <summary>
        /// File records this manifest describes
        /// </summary>
        public List<FileRecord> Records { get; set; } = new List<FileRecord>();

        /// <summary>
        /// Reads a manifest record starting from the specified stream
        /// </summary>
        /// <param name="inStream">Bag stream</param>
        /// <param name="offset">Offset into bag stream where manifest exists</param>
        /// <param name="length">Manifest length</param>
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
