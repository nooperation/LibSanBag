using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;

namespace SanBag
{
    public class Bag
    {
        private static int BagSignature => 0x66;

        public class FileRecord
        {
            public long Offset { get; set; }
            public uint Length { get; set; }
            public string Name { get; set; }
            public long TimestampNs { get; set; }
            public string BagPath { get; set; }

            public FileRecord()
            {

            }

            public FileRecord(BinaryReader in_stream)
            {
                Read(in_stream);
            }

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

        public class Manifest
        {
            public long NextManifestOffset { get; set; } = 0;
            public int NextManifestLength { get; set; } = 0;
            public List<FileRecord> Records { get; set; } = new List<FileRecord>();

            public uint Length
            {
                get
                {
                    uint length = 0;
                    foreach (var record in Records)
                    {
                        length += record.Length;
                    }
                    return length;
                }
            }

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



        /// <summary>
        /// WIP (non-functional)
        /// Creates a new bag file with the specified collection of files.
        /// </summary>
        /// <param name="output_path">Output path.</param>
        /// <param name="files_to_add">Files to add to the bag.</param>
        static public void CreateNewBag(string output_path, ICollection<string> files_to_add, ITimeProvider time_provider)
        {
            const string OffbaseString = "0fffba5e0fffba5e0fffba5e0fffba5e";

            using (var bag_stream = new BinaryWriter(File.OpenWrite(output_path)))
            {
                var next_manifest_offset = (long)0;
                var next_manifest_length = (int)0;

                bag_stream.Write(BagSignature);
                bag_stream.Write(next_manifest_offset);
                bag_stream.Write(next_manifest_length);
                bag_stream.Write((int)OffbaseString.Length + 1);
                bag_stream.Write(Encoding.ASCII.GetBytes(OffbaseString));
                bag_stream.Write(new byte[0x3F0 - 0x24]);

                if (files_to_add.Count == 0)
                {
                    return;
                }

                // First Manifest
                var manifest_begin_position = bag_stream.BaseStream.Position;
                bag_stream.Write(next_manifest_offset);
                bag_stream.Write(next_manifest_length);

                var file_offset_map = new Dictionary<string, long>();
                foreach (var path in files_to_add)
                {
                    var file_length = (uint)new FileInfo(path).Length;
                    var file_name = Path.GetFileName(path);
                    var timestamp_ns = time_provider.GetCurrentTime();

                    bag_stream.Write((byte)0xFF);

                    file_offset_map[path] = bag_stream.BaseStream.Position;
                    bag_stream.Write((long)0);
                    bag_stream.Write(file_length);
                    bag_stream.Write(timestamp_ns);

                    bag_stream.Write((int)file_name.Length);
                    bag_stream.Write(Encoding.ASCII.GetBytes(file_name));
                }

                var total_manifest_length = bag_stream.BaseStream.Position - manifest_begin_position;

                // Second pass
                //   Update file offsets in manifest
                foreach (var path in files_to_add)
                {
                    var current_offset = bag_stream.BaseStream.Position;
                    var file_offset_position = file_offset_map[path];

                    bag_stream.BaseStream.Seek(file_offset_position, SeekOrigin.Begin);
                    bag_stream.Write(current_offset);
                    bag_stream.BaseStream.Seek(current_offset, SeekOrigin.Begin);

                    using (var file_stream = File.OpenRead(path))
                    {
                        file_stream.CopyTo(bag_stream.BaseStream);
                    }
                }

                // Update the manifest length in the root
                bag_stream.BaseStream.Seek(4, SeekOrigin.Begin);
                bag_stream.Write((long)manifest_begin_position);
                bag_stream.Write((int)total_manifest_length);
            }
        }

        /// <summary>
        /// Reads the contents of a bag file.
        /// </summary>
        /// <param name="path">Path to read from.</param>
        /// <returns>Bag file contents.</returns>
        static public IDictionary<long, FileRecord> ReadBag(string path)
        {
            var file_records = new Dictionary<long, FileRecord>();

            using (var bag_stream = new BinaryReader(File.OpenRead(path)))
            {
                var bag_signature = bag_stream.ReadUInt32();
                var next_manifest_offset = bag_stream.ReadInt64();
                var next_manifest_length = bag_stream.ReadUInt32();

                while (next_manifest_offset != 0 && next_manifest_length > 0)
                {
                    var current_manifest_end = next_manifest_offset + next_manifest_length;
                    bag_stream.BaseStream.Seek(next_manifest_offset, SeekOrigin.Begin);
                    next_manifest_offset = bag_stream.ReadInt64();
                    next_manifest_length = bag_stream.ReadUInt32();

                    while (true)
                    {
                        if (bag_stream.BaseStream.Position >= current_manifest_end)
                        {
                            break;
                        }

                        if (bag_stream.ReadByte() != 0xFF)
                        {
                            break;
                        }

                        var new_record = new FileRecord(bag_stream);
                        if (file_records.ContainsKey(new_record.Offset))
                        {
                            // TODO: What do we do in this situation...
                        }
                        else
                        {
                            file_records.Add(new_record.Offset, new_record);
                        }
                    }
                }
            }

            return file_records;
        }
    }
}
