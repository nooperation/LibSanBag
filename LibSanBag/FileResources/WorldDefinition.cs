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




        private void Read_WorldDef_V4(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1411A3E10);

            reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();

        }

        private void Read_WorldDef_V7(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1411A3E20);

            reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();
        }

        private void Read_WorldDef_V2(BinaryReader reader)
        {
            var version = ReadVersion(reader, 2, 0x1411A3E30);

            if(version >= 1)
            {
                ReadString(reader);
                ReadString(reader);

                reader.ReadUInt32();
                reader.ReadUInt32();

                // m128i
                reader.ReadUInt64();
                reader.ReadUInt64();

                reader.ReadUInt32();


                if(version >= 2)
                {
                    ReadString(reader);
                }
                else
                {
                    // m128i
                    reader.ReadUInt64();
                    reader.ReadUInt64();

                    reader.ReadUInt32();
                    reader.ReadUInt32();
                }

            }
        }

        private void Read_WorldDef_V5(BinaryReader reader)
        {
            var version = ReadVersion(reader, 2, 0x1411A3E40);

            reader.ReadUInt32();
            reader.ReadUInt32();

            if (version >= 2)
            {
                reader.ReadUInt32();
            }
        }

        private void Read_WorldDef_V13(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1411A28D0);

            var count = reader.ReadUInt32();
            for (int i = 0; i < count; i++)
            {
                var data = reader.ReadUInt32();
            }

        }
        private void Read_WorldDef_V14(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1411A3E50);

            reader.ReadUInt32();
            reader.ReadUInt32();

            reader.ReadUInt64();
            reader.ReadUInt64();
            reader.ReadUInt32();

        }

        private void Read_WorldDef_Common(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1411A0F40);

            // m128i
            reader.ReadUInt64();
            reader.ReadUInt64();

            reader.ReadUInt32();

            // m128i
            reader.ReadUInt64();
            reader.ReadUInt64();

            reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();
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
            public uint Version { get; set; }
            public string NameA { get; set; }
            public string NameB { get; set; }
        }
        private AudioStream Read_AudioStream(BinaryReader reader)
        {
            var result = new AudioStream();

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

            if(Version < 4)
            {
                result.BankResource = ReadUUID(reader);
                result.InnerVersion = ReadVersion(reader, 2, 0x141160230);

                result.BackgroundEvent = ReadUUID(reader); // Only guessing this is a uuid. migght still just be 2x int64
               // result. = reader.ReadInt64();
                //result.BackgroundEvent = reader.ReadInt64();
            }

            result.BackgroundSoundResource = ReadUUID(reader);
            result.BackgroundSoundLoudness = reader.ReadSingle();

            if(result.Version >= 2)
            {
                result.UseBackgroundStream = reader.ReadBoolean();
                if(result.Version == 2)
                {
                    result.BackgroundStreamUrl = ReadString(reader);
                }
                else
                {
                    result.BackgroundStreamChannel = reader.ReadUInt32();
                }
            }

            if(result.Version >= 3)
            {
                result.AudioStreams = Read_List(reader, Read_AudioStream, 1, 0x1411AF530);
            }

            return result;
        }

        private object Read_WorldDefinitionResource(BinaryReader reader)
        {
            var Version = ReadVersion(reader, 14, 0x1410E3B90);

            if(Version < 6)
            {
                var VersionOld = reader.ReadUInt32();
            }

            var NumWorldChunksX = reader.ReadUInt32();
            var NumWorldChunksY = reader.ReadUInt32();

            var UpIdx = reader.ReadUInt32();
            var FwdIdx = reader.ReadUInt32();


            var SpawnLocations = Read_List(
                reader,
                n => ReadVectorF(n, 4),
                1,
                0x1416E9700
            );
            var SpawnFacings = Read_List(
                reader,
                n => n.ReadUInt32(),
                1,
                0x141222920
            );
            var WorldChunks = Read_List(
                reader,
                Read_WorldChunk,
                1,
                0x1411A28A0
            );
            var AudioWorldDefinition = Read_AudioWorldDefinition(reader);

            if (Version < 9)
            {
                var ScriptDefs = Read_List(reader, n => ReadComponent(n, Read_ScriptComponent), 1, 0x1416E9720);
            }
            else
            {
                var ScriptDefs = Read_List(reader, Read_ScriptComponent, 1, 0x1416E9710);
            }

            var SkyCubeMap = ReadUUID(reader);

            if(Version >= 3)
            {
                var SkyBrightness = reader.ReadUInt32();
            }


            var SkyRotationMatrix = Read_RotationMatrix(reader, 3);

            //var AtmosphereData =
            Read_WorldDef_Common(reader);

            if(Version >= 4)
            {
                // var ExposureData =
                Read_WorldDef_V4(reader);
            }
            if(Version >= 7)
            {
                // var PostEffectData
                Read_WorldDef_V7(reader);
            }
            if(Version >= 2)
            {
                // var MediaSurfaceData
                Read_WorldDef_V2(reader);
            }
            if(Version >= 5)
            {
                //var BloomData = 
                Read_WorldDef_V5(reader);
            }
            if(Version >= 6)
            {
                var GravityMagnitude =  reader.ReadUInt32();
            }
            if(Version >= 8)
            {
                var AvatarAvatarCollision = reader.ReadByte();
            }

            if(Version >= 10)
            {

                var EnableFreeCam = reader.ReadByte();
                var TeleportRangeMaximum = reader.ReadUInt32();
            }

            if(Version >= 12)
            {
                var AllowBypassSpawnPoint = reader.ReadByte();
            }

            if(Version >= 13)
            {
                // var PreloadInstanceIds =
                Read_WorldDef_V13(reader);
            }

            if(Version >= 14)
            {
                // var RuntimeInventorySettings =
                Read_WorldDef_V14(reader);
            }

            return 42;
        }

        public uint Version { get; set; }

        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var reader = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                this.Version = ReadVersion(reader, 1, 0x1411D9B90); // C3038E9E61058B48

            }
        }
    }
}
