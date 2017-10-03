using System;
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
        public enum ResourceType
        {
            AnimationBindingResource,
            BankResource,
            BlueprintResource,
            ClusterDefinition,
            EnvironmentResource,
            GeometryResourceResource,
            ModelMorphResource,
            PickResource,
            ShapeResource,
            SoundResource,
            TextureResource,
            TextureSource,
            WorldChunkDefinition,
            WorldSource,
            Unknown
        }

        public static Regex PatternRecord = new Regex(
            @"(?<hash>[a-f0-9]{32})\." +
                @"((?<image_name>.*\.png)|" +
                @"(?<resource_type>[^\.]+)" +
                    @"\.v(?<version_hash>[a-f0-9]{16})" +
                    @"\.(?<content_type>[^\.]+)" +
                    @"\.v(?<version_number>[0-9]+)" +
                    @"\.(?<unknown_type>[a-zA-Z0-9]+)" +
                @")",
            RegexOptions.ExplicitCapture);

        public long Offset { get; set; }
        public uint Length { get; set; }
        public string Name { get; set; }
        public long TimestampNs { get; set; }

        public string Hash { get; set; }
        public string ImagePath { get; set; }
        public ResourceType Type { get; set; }
        public string VersionHash { get; set; }
        public string ContentType { get; set; }
        public int VersionNumber { get; set; }
        public string Variants { get; set; }

        public bool IsRawImage => ImagePath != null;

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

            var match = PatternRecord.Match(Name);
            Hash = match.Groups["hash"].Value;
            var image_match = match.Groups["image_name"];

            if (image_match.Success)
            {
                ImagePath = image_match.Value;
            }
            else
            {
                Type = GetResourceType(match.Groups["resource_type"].Value);
                VersionHash = match.Groups["version_hash"].Value;
                ContentType = match.Groups["content_type"].Value;
                if (ContentType == "")
                {
                    Console.WriteLine("Butts");
                }
                VersionNumber = int.Parse(match.Groups["version_number"].Value);
                Variants = match.Groups["unknown_type"].Value;
            }
        }

        public static ResourceType GetResourceType(string resource_type_string)
        {
            switch (resource_type_string)
            {
                case "Bank-Resource": return ResourceType.BankResource;
                case "Blueprint-Resource": return ResourceType.BlueprintResource;
                case "Cluster-Definition": return ResourceType.ClusterDefinition;
                case "Environment-Resource": return ResourceType.EnvironmentResource;
                case "GeometryResource-Resource": return ResourceType.GeometryResourceResource;
                case "hkaAnimationBinding-Resource": return ResourceType.AnimationBindingResource;
                case "hknpShape-Resource": return ResourceType.ShapeResource;
                case "ModelMorph-Resource": return ResourceType.ModelMorphResource;
                case "Pick-Resource": return ResourceType.PickResource;
                case "Sound-Resource": return ResourceType.SoundResource;
                case "Texture-Resource": return ResourceType.TextureResource;
                case "Texture-Source": return ResourceType.TextureSource;
                case "WorldChunk-Definition": return ResourceType.WorldChunkDefinition;
                case "World-Source": return ResourceType.WorldSource;
            }

            return ResourceType.Unknown;
        }

        public override string ToString()
        {
            if (IsRawImage)
            {
                return Name;
            }

            return $"{TimestampNs} - {Hash} - {ContentType} - {Type} - {Variants} {Length} bytes";
        }
    }
}
