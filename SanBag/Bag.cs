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
        private static string OffbaseString => "0fffba5e0fffba5e0fffba5e0fffba5e";

        /// <summary>
        /// Creates a new bag file with the specified collection of files.
        /// </summary>
        /// <param name="output_path">Output path.</param>
        /// <param name="files_to_add">Files to add to the bag.</param>
        /// <param name="time_provider">Time provider.</param>
        static public void Write(string output_path, ICollection<string> files_to_add, ITimeProvider time_provider)
        {
            using (var bag_stream = new BinaryWriter(File.OpenWrite(output_path)))
            {
                // Offsets and lengths are popualted after the manifest has been written
                var next_manifest_offset = (long)0;
                var next_manifest_length = (int)0;

                bag_stream.Write(BagSignature);
                bag_stream.Write(next_manifest_offset);
                bag_stream.Write(next_manifest_length);
                bag_stream.Write((int)OffbaseString.Length + 1);
                bag_stream.Write(Encoding.ASCII.GetBytes(OffbaseString));
                bag_stream.Write(new byte[0x3CC]);

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
        static public ICollection<FileRecord> Read(Stream in_stream)
        {
            var file_records = new List<FileRecord>();

            var bag_stream = new BinaryReader(in_stream);

            var file_signature = bag_stream.ReadInt32();
            if (file_signature != BagSignature)
            {
                throw new Exception($"Invalid bag file. Expected file signature of {BagSignature}, got {file_signature}");
            }

            var manifest = new Manifest();
            manifest.Read(bag_stream, 4, 0x3FC);

            while (manifest.NextManifestOffset != 0)
            {
                var previous_manifest = manifest;

                manifest = new Manifest();
                manifest.Read(bag_stream, previous_manifest.NextManifestOffset, previous_manifest.NextManifestLength);

                file_records.AddRange(manifest.Records);
            }

            return file_records;
        }
    }
}
