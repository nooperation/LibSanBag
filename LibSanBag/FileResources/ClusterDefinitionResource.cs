using LibSanBag;
using LibSanBag.ResourceUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibSanBag.FileResources;

namespace LibSanBag.FileResources
{
    public class ClusterDefinitionResource : BaseFileResource
    {
        public override bool IsCompressed => true;

        /// <summary>
        /// Script filename
        /// </summary>
        public string Filename { get; set; }
        /// <summary>
        /// Script source code
        /// </summary>
        public string Source { get; set; }

        public static ClusterDefinitionResource Create(string version = "")
        {
            return new ClusterDefinitionResource();
        }

        public virtual string ReadString(BinaryReader decompressedStream)
        {
            var textLength = decompressedStream.ReadInt32();
            var text = new string(decompressedStream.ReadChars(textLength));
            return text;
        }


        private Tuple<ulong, ulong> ReadPair(BinaryReader reader)
        {
            var version = ReadVersion(reader, 2);

            var item1 = reader.ReadUInt64();
            var item2 = reader.ReadUInt64();

            return new Tuple<ulong, ulong>(item1, item2);
        }

        private uint ReadVersion(BinaryReader reader, uint defaultVersion)
        {
            return reader.ReadUInt32();
        }

        private string ClusterDefinition_read_name(BinaryReader reader)
        {
            //var unknown = reader.ReadInt32();

            // jsut guessing at this...
            return ReadString(reader);
        }

        private void ClusterDefinition_reader5(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1);

            // 128 bit value
            var upperA = reader.ReadUInt64();
            var lowerA = reader.ReadUInt64();

            // 96 bit value (stored in-memory as 128bit value)
            var upperB = reader.ReadUInt64();
            var lowerB = reader.ReadUInt32();
        }

        private bool rigidBodyInitialized = false;
        private void Read_RigidBody_Init(BinaryReader reader)
        {
            var pair = ReadPair(reader);
            if (pair.Item1 > 0 || pair.Item2 > 0)
            {

                rigidBodyInitialized = true;
            }
        }

        private void Read_RigidBody_post_common(BinaryReader reader)
        {
            var unused = ReadPair(reader);
        }

        private void Read_RigidBody_common_value(BinaryReader reader)
        {
            var unused = ReadPair(reader);
        }

        private void Read_RigidBody_common(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1);

            var num_values = reader.ReadUInt32();
            for (int i = 0; i < num_values; i++)
            {
                Read_RigidBody_common_value(reader);
            }
        }

        private void Read_RigidBody_v4_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 4);

            var unknownA = reader.ReadUInt32();

            ClusterDefinition_reader5(reader);
            if(version >= 2)
            {
                var unknown = reader.ReadByte();
            }
            if(version >= 3)
            {
                var version_v3 = ReadVersion(reader, 1);
                ClusterDefinition_read_name(reader);
            }
            if(version >= 4)
            {
                var unknown = reader.ReadByte();
            }
        }

        private void Read_RigidBody_v4(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1);

            var UnknownCount = reader.ReadUInt32();
            for (int i = 0; i < UnknownCount; i++)
            {
                Read_RigidBody_v4_inner(reader);
            }
        }

        private void Read_RigidBody_v6_inner_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1);

            var UnknownCount = reader.ReadUInt32();
            for (int i = 0; i < UnknownCount; i++)
            {
                var unknown = ReadPair(reader);
            }
        }

        private void Read_RigidBody_v6_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1);

            var unknownA = reader.ReadByte();
            var unknownB = reader.ReadInt32();
            var unknownC = reader.ReadInt32();

            Read_RigidBody_v6_inner_inner(reader);
        }

        private void Read_RigidBody_v6(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1);

            var UnknownCount = reader.ReadUInt32();
            for (int i = 0; i < UnknownCount; i++)
            {
                var unknownInner = reader.ReadUInt32();
                Read_RigidBody_v6_inner(reader);
            }
        }


        private void Read_RigidBody_v7_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 2);

            var unknown = reader.ReadUInt32();
            ClusterDefinition_reader5(reader);

            if(version >= 2)
            {
                var inner_version = ReadVersion(reader, 1);
                ClusterDefinition_read_name(reader);
            }
        }

        private void Read_RigidBody_v7(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1);

            var UnknownCount = reader.ReadUInt32();
            for (int i = 0; i < UnknownCount; i++)
            {
                var unknownInner = reader.ReadUInt32();
                Read_RigidBody_v7_inner(reader);
            }
        }

        private void Read_RigidBody_v10(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1);

            var UnknownCount = reader.ReadUInt32();
            for (int i = 0; i < UnknownCount; i++)
            {
                var version_inner_a = ReadVersion(reader, 1);
                ClusterDefinition_reader5(reader);
                var version_inner_b = ReadVersion(reader, 1);
                ClusterDefinition_read_name(reader);
            }
        }

        private void Read_RigidBody(BinaryReader reader)
        {
            var version = ReadVersion(reader, 13);

            // TODO: Just inline the above here
            Read_RigidBody_Init(reader);

            var length = reader.ReadUInt32();
            var something = reader.ReadBytes((int)length);

            Read_RigidBody_common(reader);
            Read_RigidBody_post_common(reader);

            if(version >= 2)
            {
                var version2_version = ReadVersion(reader, 2);
                var unknown = reader.ReadUInt32();
            }
            if(version >= 3)
            {
                var unknown = reader.ReadByte();
            }
            if(version >= 4)
            {
                Read_RigidBody_v4(reader);
            }
            if(version >= 6)
            {
                Read_RigidBody_v6(reader);
            }
            if(version >= 7)
            {
                Read_RigidBody_v7(reader);
            }
            if(version >= 8)
            {
                var unknown = reader.ReadByte();
            }
            if(version >= 9)
            {
                var version9_version = ReadVersion(reader, 1);
                ClusterDefinition_read_name(reader);
            }
            if(version >= 10)
            {
                Read_RigidBody_v10(reader);
            }
            if(version >= 12)
            {
                var unknownA = reader.ReadUInt32();
                var unknownB = reader.ReadUInt32();
            }
            if (version >= 13)
            {
                var unknown = reader.ReadUInt32();
            }
        }


        private void ClusterDefinition_read_RigidBody(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1);

            var unknown10 = reader.ReadUInt32();
            var unknown16 = reader.ReadUInt32();

            if (unknown10 != 0)
            {
                if (!rigidBodyInitialized)
                {
                    Read_RigidBody(reader);
                }
            }
        }

        private void Read_Animation_v5_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 3);

            ReadPair(reader);
            ClusterDefinition_read_name(reader);

            reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();

            if(version == 2)
            {
                reader.ReadUInt32();
            }
        }
        private void Read_Animation_v5(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1);

            var UnknownCount = reader.ReadUInt32();
            for (int i = 0; i < UnknownCount; i++)
            {
                Read_Animation_v5_inner(reader);
            }
        }


        private void Read_Animation_v8_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 2);

            ClusterDefinition_read_name(reader);
            ClusterDefinition_read_name(reader);
            ReadPair(reader);

            if(version >= 2)
            {
                ClusterDefinition_read_name(reader);
            }
        }
        private void Read_Animation_v8(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1);

            var UnknownCount = reader.ReadUInt32();
            for (int i = 0; i < UnknownCount; i++)
            {
                Read_Animation_v8_inner(reader);
            }
        }

        private void Read_Animation_v10(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1);

            var UnknownCount = reader.ReadUInt32();
            for (int i = 0; i < UnknownCount; i++)
            {
                var innerVersion = ReadVersion(reader, 1);
                ReadPair(reader); // skeleton mapper??
            }
        }

        private void Read_Animation_v11_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1);

            // m128i
            reader.ReadInt64();
            reader.ReadInt64();

            // m128i
            reader.ReadInt64();
            reader.ReadInt64();

            // m128i
            reader.ReadInt64();
            reader.ReadInt64();
        }
        private void Read_Animation_v11(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1);

            var UnknownCount = reader.ReadUInt32();
            for (int i = 0; i < UnknownCount; i++)
            {
                ReadString(reader);
                Read_Animation_v11_inner(reader);
            }
        }

        bool animationInitialized = false;
        private void Read_Animation(BinaryReader reader)
        {
            var version = ReadVersion(reader, 12);
            var pairCommon = ReadPair(reader);
            Read_RigidBody_post_common(reader);
            var pairB = ReadPair(reader);
            var pairC = ReadPair(reader);

            var unknownA = reader.ReadUInt32();
            var unknownB = reader.ReadByte();

            if(version >= 2)
            {
                reader.ReadUInt32();
                reader.ReadUInt32();
            }

            if(version >= 3)
            {
                reader.ReadUInt32();
                reader.ReadByte();
            }

            if(version < 4)
            {
                // default animation stuff
            }
            else
            {
                Read_Animation_v5(reader);
            }

            if((version - 5) <= 2)
            {
                reader.ReadUInt32();
            }

            if(version >= 6)
            {
                var version6_version = ReadVersion(reader, 1);
                ClusterDefinition_read_name(reader);
            }
            if(version >= 7)
            {
                reader.ReadUInt32();
            }
            if(version >= 8)
            {
                Read_Animation_v8(reader);
            }
            if(version >= 9)
            {
                reader.ReadUInt32();
                reader.ReadUInt32();
            }
            if(version >= 10)
            {
                Read_Animation_v10(reader);
            }

            if(version == 11)
            {
                Read_Animation_v11(reader);
            }
        }

        private void ClusterDefinition_read_Animation(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1);

            var unknownA = reader.ReadUInt32();
            if (unknownA != 0)
            {
                if (!animationInitialized)
                {
                    Read_Animation(reader);
                }
            }

        }

        private void ClusterDefinition_read_Pose(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1);
            var unknownA = reader.ReadUInt32();

            if(unknownA != 0)
            {
                // possibly flag to prevent reading version...
                var version_inner = ReadVersion(reader, 1);
            }
        }

        private void ClusterDefinition_read_Char_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 3);

            // speechAlgorithm
            ReadPair(reader);

            // speechCharacter
            ReadPair(reader);

            if(version >= 2)
            {
                var unknown = reader.ReadByte();
            }

            if(version >= 3)
            {
                var version3_version = ReadVersion(reader, 1);
                ClusterDefinition_read_name(reader);
            }
        }

        private void ClusterDefinition_read_Char(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1);
            var unknownA = reader.ReadUInt32();

            if (unknownA != 0)
            {
                ClusterDefinition_read_Char_inner(reader);
            }
        }


        private void ClusterDefinition_read_Camera_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 3);

            reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();

            if(version >= 2)
            {
                var version2_version = ReadVersion(reader, 1);
                ClusterDefinition_read_name(reader);
            }

            if(version >= 3)
            {
                reader.ReadUInt32();
                reader.ReadUInt32();
            }
        }

        private void ClusterDefinition_read_Camera(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1);


            var unknownA = reader.ReadUInt32();
            if(unknownA != 0)
            {
                ClusterDefinition_read_Camera_inner(reader);
            }
        }


        private void ClusterDefinition_read_Light_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 5);

            var unknownA = reader.ReadUInt32();

            ClusterDefinition_read_name(reader);

            // m128i
            var unknownB_hi = reader.ReadUInt64();
            var unknownB_lo = reader.ReadUInt64();

            var unknownB = reader.ReadUInt32();
            var unknownC = reader.ReadByte();

            if(version >= 3)
            {
                reader.ReadByte();
            }

            reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadByte();

            if(version >= 4)
            {
                var version4_version = ReadVersion(reader, 1);
                ClusterDefinition_read_name(reader);
            }

            if(version >= 5)
            {
                reader.ReadUInt32();
                reader.ReadUInt32();
            }
        }
        private void ClusterDefinition_read_Light(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1);

            var unknownA = reader.ReadUInt32();
            if(unknownA != 0)
            {
                ClusterDefinition_read_Light_inner(reader);
            }
        }

        private void ClusterDefinition_read_StaticMesh_inner_modelDefinition_inner_v4(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1);

            var unknownCounter = reader.ReadUInt32();
            for (int i = 0; i < unknownCounter; i++)
            {
                var version_inner = ReadVersion(reader, 1);
                reader.ReadUInt32();
                reader.ReadUInt32();
                reader.ReadUInt32();
                reader.ReadUInt32();
            }


        }

        private void ClusterDefinition_read_StaticMesh_inner_modelDefinition_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 4);

            // "material"
            ReadPair(reader);

            if(version >= 4)
            {
                // call
                ClusterDefinition_read_StaticMesh_inner_modelDefinition_inner_v4(reader);
            }
            else
            {
                var unknownB = reader.ReadUInt32();
                var unknownC = reader.ReadUInt32();
            }

            if(version < 2)
            {
                // noop
            }
            else
            {
                var unknownA = reader.ReadUInt64();
            }

            if(version < 3)
            {
                // noop
            }
            else
            {
                var unknownA = reader.ReadUInt32();
            }
        }
        private void ClusterDefinition_read_StaticMesh_inner_modelDefinition(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1);

            var unknownA = reader.ReadUInt32();
            if (unknownA != 0)
            {
                var version_inner = ReadVersion(reader, 2);

                // "geometry"
                ReadPair(reader);

                var version_inner_inner = ReadVersion(reader, 1);
                var unknownCount = reader.ReadUInt32();
                for(var i = 0; i < unknownCount; ++i)
                {
                    ClusterDefinition_read_StaticMesh_inner_modelDefinition_inner(reader);
                }
            }
        }

        private void ClusterDefinition_read_StaticMesh_inner_v3(BinaryReader reader)
        {
            var version = ReadVersion(reader, 5);

            ClusterDefinition_read_name(reader);
            ClusterDefinition_read_StaticMesh_inner_modelDefinition(reader);

            reader.ReadUInt32();

            if(version >= 2)
            {
                reader.ReadUInt32();
            }
            if(version >= 3)
            {
                reader.ReadByte();
            }
            if(version >= 4)
            {
                reader.ReadByte();
            }
            if(version >= 5)
            {
                reader.ReadUInt32();
                reader.ReadUInt32();
            }
        }

        private void ClusterDefinition_read_StaticMesh_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 3);

            if(version >= 3)
            {
                ClusterDefinition_read_StaticMesh_inner_v3(reader);
            }
            else
            {
                ClusterDefinition_read_name(reader);

                // modelDefinition
                ClusterDefinition_read_StaticMesh_inner_modelDefinition(reader);

                // skip?? TODO: Remove me most likely
                reader.ReadByte();

                if(version >= 2)
                {
                    reader.ReadUInt32();
                }
            }

        }
        private void ClusterDefinition_read_StaticMesh(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1);

            var unknownA = reader.ReadUInt32();
            if (unknownA != 0)
            {
                ClusterDefinition_read_StaticMesh_inner(reader);
            }
        }

        private void ClusterDefinition_read_RiggedMesh_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 3);

            if(version >= 3)
            {
                ClusterDefinition_read_StaticMesh_inner_v3(reader);
            }
            else
            {
                ClusterDefinition_read_name(reader);
                ClusterDefinition_read_StaticMesh_inner_modelDefinition(reader);

                // skip??? todo: remove most likely (see ClusterDefinition_read_StaticMesh_inner as well)
                reader.ReadByte();

                if(version >= 2)
                {
                    reader.ReadUInt32();
                }
            }
        }

        private void ClusterDefinition_read_RiggedMesh(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1);

            var unknownA = reader.ReadUInt32();
            if (unknownA != 0)
            {
                ClusterDefinition_read_RiggedMesh_inner(reader);
            }
        }

        private void ClusterDefinition_read_Audio_inner_preinner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1);

            var unknownA = reader.ReadUInt32();

            if((unknownA - 2) != 0)
            {
                if(unknownA == 1)
                {
                    ClusterDefinition_read_Terrain_inner_v2(reader);
                }
            }
            else
            {
                reader.ReadUInt32();
            }
        }

        private void ClusterDefinition_read_Audio_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 4);

            if(version < 3)
            {
                // bankResource
                ReadPair(reader);
            }

            Read_RigidBody_v6_inner_inner(reader);
            ClusterDefinition_read_Audio_inner_preinner(reader);


            if(version >= 2)
            {
                var version2_version = ReadVersion(reader, 1);
                ClusterDefinition_read_name(reader);
            }
            if(version >= 4)
            {
                reader.ReadUInt32();
                reader.ReadUInt32();
            }

        }

        private void ClusterDefinition_read_Audio(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1);

            var unknownA = reader.ReadUInt32();
            if (unknownA != 0)
            {
                ClusterDefinition_read_Audio_inner(reader);
            }
        }

        private void ClusterDefinition_read_Terrain_inner_v2(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1);

            // m128i
            reader.ReadUInt64();
            reader.ReadUInt64();

            // m128i
            reader.ReadUInt64();
            reader.ReadUInt64();
        }

        private void ClusterDefinition_read_Terrain_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 3);
            ClusterDefinition_read_name(reader);

            var versionB = ReadVersion(reader, 1);

            if(version >= 2)
            {
                ClusterDefinition_read_Terrain_inner_v2(reader);
            }

            if(version >= 3)
            {
                reader.ReadUInt64();
            }
        }
        private void ClusterDefinition_read_Terrain(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1);

            var unknownA = reader.ReadUInt32();
            if (unknownA != 0)
            {
                ClusterDefinition_read_Terrain_inner(reader);
            }
        }


        private void ClusterDefinition_read_IKBody(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1);

            var unknownA = reader.ReadUInt32();
            if (unknownA != 0)
            {
                var version_inner = ReadVersion(reader, 1);
            }
        }

        private void ClusterDefinition_read_Movement(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1);

            var unknownA = reader.ReadUInt32();
            if (unknownA != 0)
            {
                var version_inner = ReadVersion(reader, 1);
                ClusterDefinition_read_name(reader);
            }
        }

        private void ClusterDefinition_read_spawnPoint_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 2);

            reader.ReadByte();

            if(version >= 2)
            {
                var version_inner = ReadVersion(reader, 1);
                ClusterDefinition_read_name(reader);
            }
        }

        private void ClusterDefinition_read_spawnPoint(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1);

            var unknownA = reader.ReadUInt32();
            if (unknownA != 0)
            {
                ClusterDefinition_read_spawnPoint_inner(reader);
            }
        }


        private void ClusterDefinition_reader(BinaryReader reader)
        {
            var version = ReadVersion(reader, 5);

            ClusterDefinition_reader5(reader);
            ClusterDefinition_read_RigidBody(reader);
            ClusterDefinition_read_Animation(reader);
            ClusterDefinition_read_Pose(reader);
            ClusterDefinition_read_Char(reader);
            ClusterDefinition_read_Camera(reader);
            ClusterDefinition_read_Light(reader);
            ClusterDefinition_read_StaticMesh(reader);
            ClusterDefinition_read_RiggedMesh(reader);
            ClusterDefinition_read_Audio(reader);
            if (version >= 2)
            {
                ClusterDefinition_read_Terrain(reader);
            }
            if (version >= 3)
            {
                ClusterDefinition_read_IKBody(reader);
            }
            if (version >= 4)
            {
                ClusterDefinition_read_Movement(reader);
            }
            if (version >= 5)
            {
                ClusterDefinition_read_spawnPoint(reader);
            }
            ClusterDefinition_read_name(reader);
        }

        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var decompressedStream = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                ResourceVersion = decompressedStream.ReadInt32();

                ClusterDefinition_reader(decompressedStream);
                /*
                var unknownVersionA = decompressedStream.ReadUInt32(); // 1
                var unknownCountB = decompressedStream.ReadUInt32(); // 1
                var unknownC = decompressedStream.ReadUInt32(); // 4 (seems to change)

                var unknownD = decompressedStream.ReadUInt32(); // 1
                var unknownE = decompressedStream.ReadUInt32(); // 1

                // 96bit value [1.0 0.0 0.0] matrix row 0?
                var unknownF = decompressedStream.ReadUInt64();
                var unknownG = decompressedStream.ReadUInt32();

                // 96bit value [0.0 1.0 0.0] matrix row 1?
                var unknownH = decompressedStream.ReadUInt64();
                var unknownI = decompressedStream.ReadUInt32();

                // 96bit value [0.0 0.0 1.0] matrix row 2?
                var unknownJ = decompressedStream.ReadUInt64();
                var unknownK = decompressedStream.ReadUInt32();

                // 96bit value [0.0 0.0 0.0] unknown
                var unknownL = decompressedStream.ReadUInt64();
                var unknownM = decompressedStream.ReadUInt32();


                var unknownVersionR = decompressedStream.ReadUInt32(); // 1
                var unknownCountC = decompressedStream.ReadUInt32(); // 1

                var unknownT = decompressedStream.ReadUInt32(); // (UnknownC again)
                var unknownU = decompressedStream.ReadUInt32(); // 1
                var unknownV = decompressedStream.ReadUInt32(); // 0 (4CEFAEB3)
                var unknownW = decompressedStream.ReadUInt32(); // 0

                var unknownX = decompressedStream.ReadUInt32(); // 0

                // 128bit value (= 1.0f)
                var unknownY = decompressedStream.ReadUInt64(); // 803F
                var unknownAA = decompressedStream.ReadUInt64(); // 0

                var unknownAC = decompressedStream.ReadUInt32(); // 1
                var unknownAD = decompressedStream.ReadUInt32(); // 0 (1)
                var unknownAE = decompressedStream.ReadUInt32(); // 0 (D)

                var unknownAF = decompressedStream.ReadUInt32(); // 0
                var unknownAG = decompressedStream.ReadUInt32(); // 0
                var unknownAH = decompressedStream.ReadUInt32(); // 0
                var unknownAI = decompressedStream.ReadUInt32(); // 0

                var unknownAJ = decompressedStream.ReadUInt32(); // 0
                var unknownAK = decompressedStream.ReadUInt32(); // 0
                var unknownAL = decompressedStream.ReadUInt32(); // 0
                var unknownAM = decompressedStream.ReadUInt32(); // 0

                var unknownAN = decompressedStream.ReadUInt32(); // 0
                var unknownAO = decompressedStream.ReadUInt32(); // 0


                var Name = ReadString(decompressedStream);
                var unknownX1 = decompressedStream.ReadByte();

                var unknownBA = decompressedStream.ReadUInt32(); // 1
                var unknownBB = decompressedStream.ReadUInt32(); // 0
                var unknownBC = decompressedStream.ReadUInt32(); // 0
                var unknownBD = decompressedStream.ReadUInt32(); // 1
                var unknownBE = decompressedStream.ReadUInt32(); // 0
                var unknownBF = decompressedStream.ReadUInt32(); // 1
                var unknownBG = decompressedStream.ReadUInt32(); // 0

                var NameAgain = ReadString(decompressedStream);

                var unknownCA = decompressedStream.ReadUInt32(); // 0
                var unknownCB = decompressedStream.ReadUInt32(); // 0
                */
                Filename = "test";
                Source = "butts";
                
            }
        }
    }
}
