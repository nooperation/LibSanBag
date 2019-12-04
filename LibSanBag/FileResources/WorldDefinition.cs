using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSanBag.FileResources
{
    public class WorldDefinitionResource : BaseFileResource
    {
        public override bool IsCompressed => true;

        public static WorldDefinitionResource Create(string version = "")
        {
            return new WorldDefinitionResource();
        }

        public class ExposureData
        {
            public uint Version { get; set; }
            public float ExposureBias { get; set; }
            public float UnderexposureCorrection { get; set; }
            public float OverexposureCorrection { get; set; }
            public float UnderexposureCorrectionRate { get; set; }
            public float OverexposureCorrectionRate { get; set; }
        }
        private ExposureData Read_ExposureData(BinaryReader reader)
        {
            var result = new ExposureData();

            result.Version = ReadVersion(reader, 1, 0x1411A3E10);

            result.ExposureBias = reader.ReadSingle();
            result.UnderexposureCorrection = reader.ReadSingle();
            result.OverexposureCorrection = reader.ReadSingle();
            result.UnderexposureCorrectionRate = reader.ReadSingle();
            result.OverexposureCorrectionRate = reader.ReadSingle();

            return result;
        }

        public class PostEffectData
        {
            public uint Version { get; set; }
            public float ShadingStratification { get; set; }
            public float ShadingBandFrequency { get; set; }
            public float ShadingBandShift { get; set; }
            public float OutlineWeight { get; set; }
            public float OutlineInset { get; set; }
            public float SaturationBoost { get; set; }
        }
        private PostEffectData Read_PostEffectData(BinaryReader reader)
        {
            var result = new PostEffectData();

            result.Version = ReadVersion(reader, 1, 0x1411A3E20);
            
            result.ShadingStratification = reader.ReadSingle();
            result.ShadingBandFrequency = reader.ReadSingle();
            result.ShadingBandShift = reader.ReadSingle();
            result.OutlineWeight = reader.ReadSingle();
            result.OutlineInset = reader.ReadSingle();
            result.SaturationBoost = reader.ReadSingle();

            return result;
        }

        public class MediaSurfaceData
        {
            public uint Version { get; set; }
            public string MediaSurfaceUrl { get; set; }
            public string MediaSurfaceTargetMaterial { get; set; }
            public uint MediaSurfaceInitialWidth { get; set; }
            public uint MediaSurfaceInitialHeight { get; set; }
            public List<float> MediaSurfaceBackgroundColor { get; set; }
            public uint MediaSurfaceAlpha { get; set; }
            public string MediaSurfaceName { get; set; }
            public List<float> MediaSurfaceAudioPosition { get; set; }
            public float MediaSurfaceAudioRadius { get; set; }
            public float MediaSurfaceAudioLoudness { get; set; }
        }
        private MediaSurfaceData Read_MediaSurfaceData(BinaryReader reader)
        {
            var result = new MediaSurfaceData();

            result.Version = ReadVersion(reader, 2, 0x1411A3E30);

            if (result.Version >= 1)
            {
                result.MediaSurfaceUrl = ReadString(reader);
                result.MediaSurfaceTargetMaterial = ReadString(reader);

                result.MediaSurfaceInitialWidth = reader.ReadUInt32();
                result.MediaSurfaceInitialHeight = reader.ReadUInt32();

                result.MediaSurfaceBackgroundColor = ReadVectorF(reader, 4);

                result.MediaSurfaceAlpha = reader.ReadUInt32();


                if (result.Version >= 2)
                {
                    result.MediaSurfaceName = ReadString(reader);
                }
                else
                {
                    result.MediaSurfaceAudioPosition = ReadVectorF(reader, 4);
                    result.MediaSurfaceAudioRadius = reader.ReadSingle();
                    result.MediaSurfaceAudioLoudness = reader.ReadSingle();
                }

            }

            return result;
        }

        public class BloomData
        {
            public uint Version { get; set; }
            public float BloomStrength { get; set; }
            public float BloomWidth { get; set; }
            public float BloomWarmth { get; set; }
        }
        private BloomData Read_BloomData(BinaryReader reader)
        {
            var result = new BloomData();

            result.Version = ReadVersion(reader, 2, 0x1411A3E40);

            result.BloomStrength = reader.ReadSingle();
            result.BloomWidth = reader.ReadSingle();
             
            if (result.Version >= 2)
            {
                result.BloomWarmth = reader.ReadSingle();
            }

            return result;
        }

        public class RuntimeInventorySettings
        {
            public uint Version { get; set; }
            public uint SourceFlags { get; set; }
            public uint SpawnLifetimePolicy { get; set; }
            public ulong TotalSpawnLimit { get; set; }
            public ulong PerUserSpawnLimit { get; set; }
            public uint SpawnTimeout { get; set; }
        }
        private RuntimeInventorySettings Read_RuntimeInventorySettings(BinaryReader reader)
        {
            var result = new RuntimeInventorySettings();

            result.Version = ReadVersion(reader, 1, 0x1411A3E50);

            result.SourceFlags = reader.ReadUInt32();
            result.SpawnLifetimePolicy = reader.ReadUInt32();
            result.TotalSpawnLimit = reader.ReadUInt64();
            result.PerUserSpawnLimit = reader.ReadUInt64();
            result.SpawnTimeout = reader.ReadUInt32();

            return result;
        }

        public class AtmosphereData
        {
            public uint Version { get; set; }
            public List<float> SkyColor { get; set; }
            public float LightBiasEV { get; set; }
            public List<float> MieAlbedo { get; set; }
            public float MieGradient { get; set; }
            public float MieGradientOffset { get; set; }
            public float MieBaseDensity { get; set; }
            public float MieFarDistance { get; set; }
            public float MieAmbient { get; set; }
            public float MieAnisotropy { get; set; }
            public float RayleighFarDistance { get; set; }
            public float SecondaryScatter { get; set; }
        }
        private AtmosphereData Read_AtmosphereData(BinaryReader reader)
        {
            var result = new AtmosphereData();

            result.Version = ReadVersion(reader, 1, 0x1411A0F40);
            result.SkyColor = ReadVectorF(reader, 4);
            result.LightBiasEV = reader.ReadSingle();
            result.MieAlbedo = ReadVectorF(reader, 4);

            result.MieGradient = reader.ReadSingle();
            result.MieGradientOffset = reader.ReadSingle();
            result.MieBaseDensity = reader.ReadSingle();
            result.MieFarDistance = reader.ReadSingle();
            result.MieAmbient = reader.ReadSingle();
            result.MieAnisotropy = reader.ReadSingle();
            result.RayleighFarDistance = reader.ReadSingle();
            result.SecondaryScatter = reader.ReadSingle();

            return result;
        }

        public class WorldChunk
        {
            public uint Version { get; set; }
            public string Id { get; set; }
        }
        private WorldChunk Read_WorldChunk(BinaryReader reader)
        {
            var result = new WorldChunk();

            result.Version = ReadVersion(reader, 1, 0x141196890);
            result.Id = ReadUUID(reader);

            return result;
        }

        public class AudioStream
        {
            public uint StreamChannel { get; set; }
            public uint Version { get; set; }
            public string NameA { get; set; }
            public string NameB { get; set; }
        }
        private AudioStream Read_AudioStream(BinaryReader reader)
        {
            var result = new AudioStream();

            result.StreamChannel = reader.ReadUInt32();
            result.Version = ReadVersion(reader, 1, 0x1411C03E0);
            result.NameA = ReadString(reader);
            result.NameB = ReadString(reader);

            return result;
        }

        public class AudioWorldDefinition
        {
            public uint Version { get; set; }
            public string BankResource { get; set; }
            public uint InnerVersion { get; set; }
            public string BackgroundEvent { get; set; }

            public string BackgroundSoundResource { get; set; }
            public float BackgroundSoundLoudness { get; set; }
            public bool UseBackgroundStream { get; set; }
            public string BackgroundStreamUrl { get; set; }
            public uint BackgroundStreamChannel { get; set; }
            public List<AudioStream> AudioStreams { get; set; }
        }
        private AudioWorldDefinition Read_AudioWorldDefinition(BinaryReader reader)
        {
            var result = new AudioWorldDefinition();

            result.Version = ReadVersion(reader, 4, 0x1416E9740);

            if (result.Version < 4)
            {
                result.BankResource = ReadUUID(reader);
                result.InnerVersion = ReadVersion(reader, 2, 0x141160230);

                result.BackgroundEvent = ReadUUID(reader); // Only guessing this is a uuid. migght still just be 2x int64
                                                           // result. = reader.ReadInt64();
                                                           //result.BackgroundEvent = reader.ReadInt64();
            }

            result.BackgroundSoundResource = ReadUUID(reader);
            result.BackgroundSoundLoudness = reader.ReadSingle();

            if (result.Version >= 2)
            {
                result.UseBackgroundStream = reader.ReadBoolean();
                if (result.Version == 2)
                {
                    result.BackgroundStreamUrl = ReadString(reader);
                }
                else
                {
                    result.BackgroundStreamChannel = reader.ReadUInt32();
                }
            }

            if (result.Version >= 3)
            {
                result.AudioStreams = Read_List(reader, Read_AudioStream, 1, 0x1411AF530);
            }

            return result;
        }

        public class WorldDefinition
        {
            public uint Version { get; set; }
            public uint VersionOld { get; set; }
            public uint NumWorldChunksX { get; set; }
            public uint NumWorldChunksY { get; set; }
            public uint UpIdx { get; set; }
            public uint FwdIdx { get; set; }
            public List<List<float>> SpawnLocations { get; set; }
            public List<float> SpawnFacings { get; set; }
            public List<WorldChunk> WorldChunks { get; set; }
            public AudioWorldDefinition AudioWorldDefinition { get; set; }
            public List<ClusterDefinitionResource.ScriptComponent> ScriptDefs { get; set; }
            public string SkyCubemap { get; set; }
            public float SkyBrightness { get; set; }
            public List<List<float>> SkyRotationMatrix { get; set; }
            public AtmosphereData AtmosphereData { get; set; }
            public ExposureData ExposureData { get; set; }
            public PostEffectData PostEffectData { get; set; }
            public MediaSurfaceData MediaSurfaceData { get; set; }
            public BloomData BloomData { get; set; }
            public float GravityMagnitude { get; set; }
            public bool AvatarAvatarCollision { get; set; }
            public bool EnableFreeCamera { get; set; }
            public float TeleportRangeMaximum { get; set; }
            public bool AllowBypassSpawnPoint { get; set; }
            public List<uint> PreloadInstanceIds { get; set; }
            public RuntimeInventorySettings RuntimeInventorySettings { get; set; }
        }
        private WorldDefinition Read_WorldDefinitionResource(BinaryReader reader)
        {
            var result = new WorldDefinition();

            result.Version = ReadVersion(reader, 14, 0x1410E3B90);

            if (result.Version < 6)
            {
                result.VersionOld = reader.ReadUInt32();
            }

            result.NumWorldChunksX = reader.ReadUInt32();
            result.NumWorldChunksY = reader.ReadUInt32();

            result.UpIdx = reader.ReadUInt32();
            result.FwdIdx = reader.ReadUInt32();

            result.SpawnLocations = Read_List(
                reader,
                n => ReadVectorF(n, 4),
                1,
                0x1416E9700
            );
            result.SpawnFacings = Read_List(
                reader,
                n => n.ReadSingle(),
                1,
                0x141222920
            );
            result.WorldChunks = Read_List(
                reader,
                Read_WorldChunk,
                1,
                0x1411A28A0
            );
            result.AudioWorldDefinition = Read_AudioWorldDefinition(reader);

            if (result.Version < 9)
            {
                result.ScriptDefs = Read_List(reader, n => ReadComponent(n, tempReader.Read_ScriptComponent), 1, 0x1416E9720);
            }
            else
            {
                result.ScriptDefs = Read_List(reader, tempReader.Read_ScriptComponent, 1, 0x1416E9710);
            }

            result.SkyCubemap = ReadUUID(reader);

            if (result.Version >= 3)
            {
                result.SkyBrightness = reader.ReadSingle();
            }

            result.SkyRotationMatrix = Read_RotationMatrix(reader, 3);
            result.AtmosphereData = Read_AtmosphereData(reader);

            if (result.Version >= 4)
            {
                result.ExposureData = Read_ExposureData(reader);
            }
            if (result.Version >= 7)
            {
                result.PostEffectData = Read_PostEffectData(reader);
            }
            if (result.Version >= 2)
            {
                result.MediaSurfaceData = Read_MediaSurfaceData(reader);
            }
            if (result.Version >= 5)
            {
                result.BloomData = Read_BloomData(reader);
            }
            if (result.Version >= 6)
            {
                result.GravityMagnitude = reader.ReadSingle();
            }
            if (result.Version >= 8)
            {
                result.AvatarAvatarCollision = reader.ReadBoolean();
            }

            if (result.Version >= 10)
            {
                result.EnableFreeCamera = reader.ReadBoolean();
                result.TeleportRangeMaximum = reader.ReadSingle();
            }

            if (result.Version >= 12)
            {
                result.AllowBypassSpawnPoint = reader.ReadBoolean();
            }

            if (result.Version >= 13)
            {
                result.PreloadInstanceIds = Read_List(reader, n => n.ReadUInt32(), 1, 0x1411A28D0);
            }

            if (result.Version >= 14)
            {
                result.RuntimeInventorySettings = Read_RuntimeInventorySettings(reader);
            }

            return result;
        }

        public WorldDefinition Resource { get; set; }

        private ClusterDefinitionResource tempReader;
        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            tempReader = new ClusterDefinitionResource();
            tempReader.OverrideVersionMap(this.versionMap);

            using (var reader = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                this.Resource = Read_WorldDefinitionResource(reader);
            }
        }
    }
}
