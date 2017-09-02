using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSanBag
{
    public class FileRecord
    {
        public long Offset { get; set; }
        public uint Length { get; set; }
        public string Name { get; set; }
        public long TimestampNs { get; set; }

        public FileRecord(BinaryReader in_stream)
        {
            Read(in_stream);
        }

        /// <summary>
        /// Saves the file record to the specified path.
        /// </summary>
        /// <param name="path">Output path.</param>
        public void Save(Stream in_stream, Stream out_stream, Action<FileRecord, uint> ReportProgress = null, Func<bool> ShouldCancel = null)
        {
            in_stream.Seek(Offset, SeekOrigin.Begin);
            var bytes_remaining = Length;
            var buffer = new byte[32767];

            while (bytes_remaining > 0)
            {
                if (ShouldCancel != null && ShouldCancel())
                {
                    throw new Exception("FileRecord::Save() aborted");
                }

                var bytes_to_read = bytes_remaining > buffer.Length ? buffer.Length : (int)bytes_remaining;
                var bytes_read = in_stream.Read(buffer, 0, bytes_to_read);

                out_stream.Write(buffer, 0, bytes_read);

                bytes_remaining -= (uint)bytes_read;
                ReportProgress?.Invoke(this, Length - bytes_remaining);
            }
        }

        public void Read(BinaryReader in_stream)
        {
            Offset = in_stream.ReadInt64();
            Length = in_stream.ReadUInt32();
            TimestampNs = in_stream.ReadInt64();

            var name_length = in_stream.ReadInt32();
            Name = new string(in_stream.ReadChars(name_length));
        }

        public override string ToString()
        {
            return $"{TimestampNs} - {Name} - {Length} bytes";
        }
    }
}
