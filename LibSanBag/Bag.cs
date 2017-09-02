using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;

namespace LibSanBag
{
    public class Bag
    {
        private static int BagSignature => 0x66;
        private static string OffbaseString => "0fffba5e0fffba5e0fffba5e0fffba5e";

        /// <summary>
        /// Creates a new bag file with the specified collection of files.
        /// </summary>
        /// <param name="outStream">Output stream.</param>
        /// <param name="filesToAdd">Files to add to the bag.</param>
        /// <param name="timeProvider">Time provider.</param>
        static public void Write(Stream outStream, ICollection<string> filesToAdd, ITimeProvider timeProvider)
        {
            var bagStream = new BinaryWriter(outStream);

            // Offsets and lengths are popualted after the manifest has been written
            var nextManifestOffset = (long)0;
            var nextManifestLength = (int)0;

            bagStream.Write(BagSignature);
            bagStream.Write(nextManifestOffset);
            bagStream.Write(nextManifestLength);
            bagStream.Write((int)OffbaseString.Length + 1);
            bagStream.Write(Encoding.ASCII.GetBytes(OffbaseString));
            bagStream.Write(new byte[0x3CC]);

            if (filesToAdd.Count == 0)
            {
                return;
            }

            // First Manifest
            var manifestBeginPosition = bagStream.BaseStream.Position;
            bagStream.Write(nextManifestOffset);
            bagStream.Write(nextManifestLength);

            var fileOffsetMap = new Dictionary<string, long>();
            foreach (var path in filesToAdd)
            {
                var fileLength = (uint)new FileInfo(path).Length;
                var fileName = Path.GetFileName(path);
                var timestampNs = timeProvider.GetCurrentTime();

                bagStream.Write((byte)0xFF);

                fileOffsetMap[path] = bagStream.BaseStream.Position;
                bagStream.Write((long)0);
                bagStream.Write(fileLength);
                bagStream.Write(timestampNs);

                bagStream.Write((int)fileName.Length);
                bagStream.Write(Encoding.ASCII.GetBytes(fileName));
            }

            var totalManifestLength = bagStream.BaseStream.Position - manifestBeginPosition;

            // Second pass
            //   Update file offsets in manifest
            foreach (var path in filesToAdd)
            {
                var currentOffset = bagStream.BaseStream.Position;
                var fileOffestPosition = fileOffsetMap[path];

                bagStream.BaseStream.Seek(fileOffestPosition, SeekOrigin.Begin);
                bagStream.Write(currentOffset);
                bagStream.BaseStream.Seek(currentOffset, SeekOrigin.Begin);

                using (var fileStream = File.OpenRead(path))
                {
                    fileStream.CopyTo(bagStream.BaseStream);
                }
            }

            // Update the manifest length in the root
            bagStream.BaseStream.Seek(4, SeekOrigin.Begin);
            bagStream.Write((long)manifestBeginPosition);
            bagStream.Write((int)totalManifestLength);
        }

        /// <summary>
        /// Reads the contents of a bag file.
        /// </summary>
        /// <param name="path">Path to read from.</param>
        /// <returns>Bag file contents.</returns>
        static public ICollection<FileRecord> Read(Stream inStream)
        {
            var fileRecords = new List<FileRecord>();

            var bagStream = new BinaryReader(inStream);

            var fileSignature = bagStream.ReadInt32();
            if (fileSignature != BagSignature)
            {
                throw new Exception($"Invalid bag file. Expected file signature of {BagSignature}, got {fileSignature}");
            }

            var manifest = new Manifest();
            manifest.Read(bagStream, 4, 0x3FC);

            while (manifest.NextManifestOffset != 0)
            {
                var previousManifest = manifest;

                manifest = new Manifest();
                manifest.Read(bagStream, previousManifest.NextManifestOffset, previousManifest.NextManifestLength);

                fileRecords.AddRange(manifest.Records);
            }

            return fileRecords;
        }
    }
}
