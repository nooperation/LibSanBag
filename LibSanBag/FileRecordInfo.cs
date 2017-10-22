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
            BankResource,
            BlueprintResource,
            ClusterDefinition,
            EnvironmentResource,
            GeometryResourceResource,
            ModelMorphResource,
            PickResource,
            SoundResource,
            TextureResource,
            TextureSource,
            WorldChunkDefinition,
            WorldSource,
            ScriptMetadataResource,
            ScriptCompiledBytecodeResource,
            ScriptSourceTextResource,
            ClusterSource,
            WorldChunkSource,
            LicenseResource,
            WorldDefinition,
            AudioMaterialResource,
            MaterialResource,
            ScriptResource,
            hkaAnimationBindingResource,
            hkaSkeletonResource,
            hknpMaterialResource,
            hknpPhysicsSystemDataResource,
            hknpShapeResource,
            FileResource,
            TextureImport,
            GeometryResourceCanonical,
            GeometryResourceImport,
            SoundImport,
            hkbBehaviorGraphResource,
            hkbCharacterDataResource,
            LuaScriptResource,
            AudioGraphResource,
            TerrainRuntimeTextureData,
            TerrainRuntimeDecalTextureData,
            BehaviorProjectData,
            hkbProjectDataResource,
            PickableModelResource,
            SpeechGraphicsEngineResource,
            SpeechGraphicsAnimationResource,
            VertexDefinitionResourceResource,
            BufferResource,
            MeshResource,
            TerrainRuntimeData,
            TerrainSourceData,
            hkpConstraintDataResource,
            hkaSkeletonMapperResource,
            hknpRagdollDataResource,
            TestResource,
            HeightmapImport,
            hknpShapeImport,
            AnimationCanonical,
            AnimationImport,
            Unknown,

        }

        public enum PayloadType
        {
            Payload,
            Unknown
        }

        public enum VariantType
        {
            NoVariants,     // "noVariants"
            Server,         // "server"
            PcClient,       // "pcClient"
            Payload,        // "payload"
            Manifest,       // "manifest"
            Debug,          // "debug"
            Capabilities,   // "capabilities"
            Null,           // "null"
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
                case "ModelMorph-Resource": return ResourceType.ModelMorphResource;
                case "Pick-Resource": return ResourceType.PickResource;
                case "Sound-Resource": return ResourceType.SoundResource;
                case "Texture-Resource": return ResourceType.TextureResource;
                case "Texture-Source": return ResourceType.TextureSource;
                case "WorldChunk-Definition": return ResourceType.WorldChunkDefinition;
                case "World-Source": return ResourceType.WorldSource;
                case "ScriptMetadata-Resource": return ResourceType.ScriptMetadataResource;
                case "ScriptCompiledBytecode-Resource": return ResourceType.ScriptCompiledBytecodeResource;
                case "ScriptSourceText-Resource": return ResourceType.ScriptSourceTextResource;
                case "Cluster-Source": return ResourceType.ClusterSource;
                case "WorldChunk-Source": return ResourceType.WorldChunkSource;
                case "License-Resource": return ResourceType.LicenseResource;
                case "World-Definition": return ResourceType.WorldDefinition;
                case "AudioMaterial-Resource": return ResourceType.AudioMaterialResource;
                case "Material-Resource": return ResourceType.MaterialResource;
                case "Script-Resource": return ResourceType.ScriptResource;
                case "hkaAnimationBinding-Resource": return ResourceType.hkaAnimationBindingResource;
                case "hkaSkeleton-Resource": return ResourceType.hkaSkeletonResource;
                case "hknpMaterial-Resource": return ResourceType.hknpMaterialResource;
                case "hknpPhysicsSystemData-Resource": return ResourceType.hknpPhysicsSystemDataResource;
                case "hknpShape-Resource": return ResourceType.hknpShapeResource;
                case "File-Resource": return ResourceType.FileResource;
                case "Texture-Import": return ResourceType.TextureImport;
                case "GeometryResource-Canonical": return ResourceType.GeometryResourceCanonical;
                case "GeometryResource-Import": return ResourceType.GeometryResourceImport;
                case "Sound-Import": return ResourceType.SoundImport;
                case "hkbBehaviorGraph-Resource": return ResourceType.hkbBehaviorGraphResource;
                case "hkbCharacterData-Resource": return ResourceType.hkbCharacterDataResource;
                case "LuaScript-Resource": return ResourceType.LuaScriptResource;
                case "AudioGraph-Resource": return ResourceType.AudioGraphResource;
                case "Terrain-RuntimeTextureData": return ResourceType.TerrainRuntimeTextureData;
                case "Terrain-RuntimeDecalTextureData": return ResourceType.TerrainRuntimeDecalTextureData;
                case "BehaviorProject-Data": return ResourceType.BehaviorProjectData;
                case "hkbProjectData-Resource": return ResourceType.hkbProjectDataResource;
                case "PickableModel-Resource": return ResourceType.PickableModelResource;
                case "SpeechGraphicsEngine-Resource": return ResourceType.SpeechGraphicsEngineResource;
                case "SpeechGraphicsAnimation-Resource": return ResourceType.SpeechGraphicsAnimationResource;
                case "VertexDefinitionResource-Resource": return ResourceType.VertexDefinitionResourceResource;
                case "Buffer-Resource": return ResourceType.BufferResource;
                case "Mesh-Resource": return ResourceType.MeshResource;
                case "Terrain-RuntimeData": return ResourceType.TerrainRuntimeData;
                case "Terrain-SourceData": return ResourceType.TerrainSourceData;
                case "hkpConstraintData-Resource": return ResourceType.hkpConstraintDataResource;
                case "hkaSkeletonMapper-Resource": return ResourceType.hkaSkeletonMapperResource;
                case "hknpRagdollData-Resource": return ResourceType.hknpRagdollDataResource;
                case "Test-Resource": return ResourceType.TestResource;
                case "Heightmap-Import": return ResourceType.HeightmapImport;
                case "hknpShape-Import": return ResourceType.hknpShapeImport;
                case "Animation-Canonical": return ResourceType.AnimationCanonical;
                case "Animation-Import": return ResourceType.AnimationImport;
            }

            return ResourceType.Unknown;
        }
    }
}
