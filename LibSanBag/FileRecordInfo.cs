using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LibSanBag
{
    public class FileRecordInfo
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

        public enum PayloadType
        {
            Payload,
            Unknown
        }

        public enum VariantType
        {
            NoVariants,
            PcClient,
            Unknown
        }

        public static Regex PatternRecord = new Regex(
            @"(?<hash>[a-f0-9]{32})\." +
                @"((?<image_name>.*\.png)|" +
                @"(?<resource_type>[^\.]+)" +
                    @"\.v(?<version_hash>[a-f0-9]{16})" +
                    @"\.(?<payload_type>[^\.]+)" +
                    @"\.v(?<version_number>[0-9]+)" +
                    @"\.(?<variants_type>[a-zA-Z0-9]+)" +
                @")",
            RegexOptions.ExplicitCapture);

        public string Hash { get; set; }
        public string ImagePath { get; set; }
        public ResourceType Resource { get; set; }
        public string VersionHash { get; set; }
        public PayloadType Payload { get; set; }
        public int VersionNumber { get; set; }
        public VariantType Variant { get; set; }

        public bool IsRawImage => ImagePath != null;

        public static FileRecordInfo Create(string filename)
        {
            var match = PatternRecord.Match(filename);
            if (match.Success == false)
            {
                return null;
            }

            var result = new FileRecordInfo();
            result.Hash = match.Groups["hash"].Value;
            var image_match = match.Groups["image_name"];

            if (image_match.Success)
            {
                result.ImagePath = image_match.Value;
            }
            else
            {
                result.Resource = GetResourceType(match.Groups["resource_type"].Value);
                result.VersionHash = match.Groups["version_hash"].Value;
                result.Payload = GetPayloadType(match.Groups["payload_type"].Value);
                result.VersionNumber = int.Parse(match.Groups["version_number"].Value);
                result.Variant = GetVariantType(match.Groups["variants_type"].Value);
            }

            return result;
        }

        public static VariantType GetVariantType(string variant_type_string)
        {
            switch (variant_type_string)
            {
                case "noVariants": return VariantType.NoVariants;
                case "pcClient": return VariantType.PcClient;
            }

            return VariantType.Unknown;
        }

        public static PayloadType GetPayloadType(string payload_type_string)
        {
            switch (payload_type_string)
            {
                case "payload": return PayloadType.Payload;
            }

            return PayloadType.Unknown;
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
    }
}
