﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LibSanBag
{
    public class FileRecord
    {
        public long Offset { get; private set; }
        public uint Length { get; private set; }
        public string Name { get; private set; }
        public long TimestampNs { get; private set; }

        public FileRecordInfo Info { get; private set; }

        public FileRecord(BinaryReader inStream)
        {
            Read(inStream);
        }

        /// <summary>
        /// Saves the file record to the specified path.
        /// </summary>
        /// <param name="path">Output path.</param>
        public void Save(Stream inStream, Stream outStream, Action<FileRecord, uint> ReportProgress = null, Func<bool> ShouldCancel = null)
        {
            inStream.Seek(Offset, SeekOrigin.Begin);
            var bytesRemaining = Length;
            var buffer = new byte[32767];

            while (bytesRemaining > 0)
            {
                if (ShouldCancel != null && ShouldCancel())
                {
                    throw new Exception("FileRecord::Save() aborted");
                }

                var bytesToRead = bytesRemaining > buffer.Length ? buffer.Length : (int)bytesRemaining;
                var bytesRead = inStream.Read(buffer, 0, bytesToRead);

                outStream.Write(buffer, 0, bytesRead);

                bytesRemaining -= (uint)bytesRead;
                ReportProgress?.Invoke(this, Length - bytesRemaining);
            }
        }

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
