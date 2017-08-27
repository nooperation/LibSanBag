using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SanBag
{
    class Bag
    {
        private static int BagSignature => 0x66;

        public class FileRecord
        {
            public long Offset { get; set; }
            public uint Length { get; set; }
            public string Name { get; set; }
            public ulong Unknown { get; set; }
            public string BagPath { get; set; }

            /// <summary>
            /// Saves the file record to the specified path.
            /// </summary>
            /// <param name="path">Output path.</param>
            public void Save(string path)
            {
                using (var bag_stream = File.OpenRead(BagPath))
                {
                    using (var output_stream = File.OpenWrite(path))
                    {
                        bag_stream.Seek(Offset, SeekOrigin.Begin);
                        var bytes_remaining = Length;
                        var buffer = new byte[32767];

                        while (bytes_remaining > 0)
                        {
                            var bytes_to_read = bytes_remaining > buffer.Length ? buffer.Length : (int)bytes_remaining;
                            var bytes_read = bag_stream.Read(buffer, 0, bytes_to_read);

                            output_stream.Write(buffer, 0, bytes_read);

                            bytes_remaining -= (uint)bytes_read;
                        }
                    }
                }
            }

            public override string ToString()
            {
                return $"{Offset} - {Name} - {Length} bytes";
            }
        }

        /// <summary>
        /// WIP (non-functional)
        /// Creates a new bag file with the specified collection of files.
        /// </summary>
        /// <param name="output_path">Output path.</param>
        /// <param name="files_to_add">Files to add to the bag.</param>
        static public void CreateNewBag(string output_path, ICollection<string> files_to_add)
        {
            using (var bag_stream = new BinaryWriter(File.OpenWrite(output_path)))
            {
                bag_stream.Write(BagSignature);
                if (files_to_add.Count == 0)
                {
                    bag_stream.Write((long)0);
                    bag_stream.Write((int)0);
                    bag_stream.Write(new byte[0x3F0]);
                    return;
                }

                var next_collection_offset = 0x400;
                var next_collection_flags = 0x00100000;
                var filler = new byte[0x3F0];

                // Bag header
                bag_stream.Write((long)next_collection_offset);
                bag_stream.Write((int)next_collection_flags);
                bag_stream.Write(filler);

                // File collection header
                next_collection_offset = 0;
                next_collection_flags = 0;
                bag_stream.Write((long)next_collection_offset);
                bag_stream.Write((int)next_collection_flags);

                var offset_mapping = new Dictionary<string, long>();
                var file_index = 1;

                // First pass
                //   Write file headers
                foreach (var path in files_to_add)
                {
                    var file_offset = 0;
                    var file_length = new FileInfo(path).Length;
                    var unknown = file_index*1000000; // TODO: This needs to be known...
                    var FilenameBytes = Encoding.ASCII.GetBytes(Path.GetFileName(path));

                    bag_stream.Write((byte)0xFF);

                    offset_mapping.Add(path, bag_stream.BaseStream.Position);

                    bag_stream.Write((ulong)file_offset);
                    bag_stream.Write((uint)file_length);
                    bag_stream.Write((ulong)unknown);
                    bag_stream.Write((uint)FilenameBytes.Length);
                    bag_stream.Write(FilenameBytes);

                    ++file_index;
                }

                // Second pass
                //   Write file contents and update offsets in file headers
                foreach (var path in files_to_add)
                {
                    var current_offset = bag_stream.BaseStream.Position;
                    var file_header_offset = offset_mapping[path];

                    bag_stream.BaseStream.Seek(file_header_offset, SeekOrigin.Begin);
                    bag_stream.Write(current_offset);
                    bag_stream.BaseStream.Seek(current_offset, SeekOrigin.Begin);

                    using (var file_stream = File.OpenRead(path))
                    {
                        file_stream.CopyTo(bag_stream.BaseStream);
                    }
                }
            }
        }

        /// <summary>
        /// Reads the contents of a bag file.
        /// </summary>
        /// <param name="path">Path to read from.</param>
        /// <returns>Bag file contents.</returns>
        static public IDictionary<long, FileRecord> ReadBag(string path)
        {
            var file_records = new SortedDictionary<long, FileRecord>();

            using (var bag_stream = new BinaryReader(File.OpenRead(path)))
            {
                var bag_signature = bag_stream.ReadUInt32();

                while (true)
                {
                    var next_offset = bag_stream.ReadInt64();
                    var next_offset_flags = bag_stream.ReadUInt32();

                    while (bag_stream.ReadByte() == 0xFF)
                    {
                        var file_offset = bag_stream.ReadInt64();
                        var file_length = bag_stream.ReadUInt32();
                        var unknown = bag_stream.ReadUInt64();

                        var FilenameLength = bag_stream.ReadInt32();
                        var Filename = new string(bag_stream.ReadChars(FilenameLength));

                        var new_record = new FileRecord()
                        {
                            Unknown = unknown,
                            Length = file_length,
                            Name = Filename,
                            Offset = file_offset,
                            BagPath = path
                        };

                        if (file_records.ContainsKey(file_offset))
                        {
                            // TODO: What do we do in this situation...
                        }
                        else
                        {
                            file_records.Add(file_offset, new_record);
                        }
                    }

                    if (next_offset == 0)
                    {
                        break;
                    }

                    bag_stream.BaseStream.Seek(next_offset, SeekOrigin.Begin);
                }
            }

            return file_records;
        }
    }
}
