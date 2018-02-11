using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace LibSanBag
{
    public class FileRecord
    {
        /// <summary>
        /// File offset in the Bag
        /// </summary>
        public long Offset { get; set; }
        /// <summary>
        /// File length
        /// </summary>
        public uint Length { get; set; }
        /// <summary>
        /// File name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// File timestamp in nanoseconds
        /// </summary>
        public long TimestampNs { get; set; }
        /// <summary>
        /// Extended file info. May be Null.
        /// </summary>
        [CanBeNull]
        public FileRecordInfo Info { get; set; }

        public FileRecord()
        {

        }

        /// <summary>
        /// Creates a new FileRecord using the specified Bag stream.
        /// </summary>
        /// <param name="inStream">Stream containing FileRecord data. Assumes stream is already positioned to the FileRecord data</param>
        public FileRecord(BinaryReader inStream)
        {
            Read(inStream);
        }

        /// <summary>
        /// Saves a file described by this FileRecord to the specified output stream.
        /// </summary>
        /// <param name="inStream">Bag stream containing this FileRecord</param>
        /// <param name="outStream">Output stream where file described by this FileRecord will be written to</param>
        /// <param name="ReportProgress">Callback function to report write process</param>
        /// <param name="ShouldCancel">Function to check to see if the write process should be aborted</param>
        public void Save(Stream inStream, Stream outStream, Action<FileRecord, uint> ReportProgress = null, Func<bool> ShouldCancel = null)
        {
            inStream.Seek(Offset, SeekOrigin.Begin);
            var bytesRemaining = Length;
            var buffer = new byte[32767];

            while (bytesRemaining > 0)
            {
                if (ShouldCancel != null && ShouldCancel())
                {
                    throw new Exception("Failed to save: FileRecord::Save() aborted");
                }

                var bytesToRead = bytesRemaining > buffer.Length ? buffer.Length : (int)bytesRemaining;
                var bytesRead = inStream.Read(buffer, 0, bytesToRead);
                if (bytesRead == 0)
                {
                    throw new Exception("Failed to save: Reached unexpected end of stream");
                }

                outStream.Write(buffer, 0, bytesRead);

                bytesRemaining -= (uint)bytesRead;
                ReportProgress?.Invoke(this, Length - bytesRemaining);
            }
        }

        /// <summary>
        /// Reads a file record from a Bag stream.
        /// </summary>
        /// <param name="inStream">Stream containing FileRecord data. Assumes stream is already positioned to the FileRecord data</param>
        public void Read(BinaryReader inStream)
        {
            Offset = inStream.ReadInt64();
            Length = inStream.ReadUInt32();
            TimestampNs = inStream.ReadInt64();

            var nameLength = inStream.ReadInt32();
            Name = new string(inStream.ReadChars(nameLength));

            Info = FileRecordInfo.Create(Name);
        }

        public override string ToString()
        {
            if (Info == null || Info.IsRawImage)
            {
                return Name;
            }

            return $"{TimestampNs} - {Info.Hash} - {Info.Payload} - {Info.Resource} - {Info.Variant} {Length} bytes";
        }
    }
}
