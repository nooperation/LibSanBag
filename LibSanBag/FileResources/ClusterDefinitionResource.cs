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

            Console.WriteLine($"ReadString: {text}");
            return text;
        }


        private string ReadUUID(BinaryReader reader)
        {
            var version = ReadVersion(reader, 2, 0x141196890);

            var item1 = reader.ReadUInt64();
            var item2 = reader.ReadUInt64();

            var left = BitConverter.GetBytes(item1);
            var right = BitConverter.GetBytes(item2);

            StringBuilder sb = new StringBuilder();
            for (int i = left.Length-1; i >= 0; i--)
            {
                sb.AppendFormat("{0:x2}", left[i]);
            }
            for (int i = right.Length-1; i >= 0; i--)
            {
                sb.AppendFormat("{0:x2}", right[i]);
            }

            var uuid = sb.ToString();
            Console.WriteLine($"ReadUUID: {uuid}");

            return uuid;
        }

        private Dictionary<ulong, uint> versionMap = new Dictionary<ulong, uint>();
        private uint ReadVersion(BinaryReader reader, uint defaultVersion, ulong? versionType)
        {
            if (versionType != null && versionMap.ContainsKey(versionType.Value))
            {
                return versionMap[versionType.Value];
            }

            var version = reader.ReadUInt32();

            if (versionType != null)
            {
                versionMap[versionType.Value] = version;
            }

            return version;
        }

        private string ClusterDefinition_read_name(BinaryReader reader)
        {
            //var unknown = reader.ReadInt32();

            // jsut guessing at this...
            var str = ReadString(reader);
            return str;
        }

        private void ClusterDefinition_reader5(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1411A0F00);

            // 128 bit value
            var upperA = reader.ReadUInt64();
            var lowerA = reader.ReadUInt64();

            // 96 bit value (stored in-memory as 128bit value)
            var upperB = reader.ReadUInt64();
            var lowerB = reader.ReadUInt32();
        }

        private void Read_RigidBody_Init(BinaryReader reader)
        {
            var bodyResourceHandleUUID = ReadUUID(reader);
        }

        private void Read_RigidBody_post_common(BinaryReader reader)
        {
            var varProxyShapeUUID = ReadUUID(reader);
        }

        private void Read_RigidBody_common_value(BinaryReader reader)
        {
            var valueUUID = ReadUUID(reader);
        }

        private void Read_RigidBody_common(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x14170C770);

            var num_values = reader.ReadUInt32();
            for (int i = 0; i < num_values; i++)
            {
                Read_RigidBody_common_value(reader);
            }
        }

        private void Read_RigidBody_v4_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 4, 0x14170FBC0);

            var unknownA = reader.ReadUInt32();

            ClusterDefinition_reader5(reader);
            if(version >= 2)
            {
                var unknown = reader.ReadByte();
            }
            if(version >= 3)
            {
                var version_v3 = ReadVersion(reader, 1, 0x1416F4E40);
                var name = ClusterDefinition_read_name(reader);
            }
            if(version >= 4)
            {
                var unknown = reader.ReadByte();
            }
        }

        private void Read_RigidBody_v4(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x14170CCA0);

            var UnknownCount = reader.ReadUInt32();
            for (int i = 0; i < UnknownCount; i++)
            {
                Read_RigidBody_v4_inner(reader);
            }
        }

        private void Read_RigidBody_v6_inner_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x14121C2C0);

            var UnknownCount = reader.ReadUInt32();
            for (int i = 0; i < UnknownCount; i++)
            {
                var valueUUID = ReadUUID(reader);
            }
        }

        private void Read_RigidBody_v6_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x14120B5B0);

            var unknownA = reader.ReadByte();
            var unknownB = reader.ReadInt32();
            var unknownC = reader.ReadInt32();

            Read_RigidBody_v6_inner_inner(reader);
        }

        private void Read_RigidBody_v6(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x141203830);

            var UnknownCount = reader.ReadUInt32();
            for (int i = 0; i < UnknownCount; i++)
            {
                var unknownInner = reader.ReadUInt32();
                Read_RigidBody_v6_inner(reader);
            }
        }


        private void Read_RigidBody_v7_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 2, 0x14170FBD0);

            var unknown = reader.ReadUInt32();
            ClusterDefinition_reader5(reader);

            if(version >= 2)
            {
                var inner_version = ReadVersion(reader, 1, 0x1416F4E40);
                var name = ClusterDefinition_read_name(reader);
            }
        }

        private void Read_RigidBody_v7(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x14170CCB0);

            var UnknownCount = reader.ReadUInt32();
            for (int i = 0; i < UnknownCount; i++)
            {
                Read_RigidBody_v7_inner(reader);
            }
        }

        private void Read_RigidBody_v10(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x14170CCC0);

            var UnknownCount = reader.ReadUInt32();
            for (int i = 0; i < UnknownCount; i++)
            {
                var version_inner_a = ReadVersion(reader, 1, 0x14170FBE0);
                ClusterDefinition_reader5(reader);

                var version_inner_b = ReadVersion(reader, 1, 0x1416F4E40);
                var name = ClusterDefinition_read_name(reader);
            }
        }

        private void Read_RigidBody(BinaryReader reader)
        {
            var version = ReadVersion(reader, 13, 0x1417056C0);

            // TODO: Just inline the above here
            Read_RigidBody_Init(reader);

            var length = reader.ReadUInt32();
            var something = reader.ReadBytes((int)length);

            Read_RigidBody_common(reader);
            Read_RigidBody_post_common(reader);

            if(version >= 2)
            {
                var version2_version = ReadVersion(reader, 2, 0x141196890);
                var unknownA = reader.ReadUInt64();
                var unknownB = reader.ReadUInt64();
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
                var version9_version = ReadVersion(reader, 1, 0x1416F4E40);
                var name = ClusterDefinition_read_name(reader);
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
            var version = ReadVersion(reader, 1, 0x1413A0990);

            var unknownA = reader.ReadUInt32();
            if (unknownA != 0)
            {
                Read_RigidBody(reader);
            }
        }

        private void Read_Animation_v5_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 3, 0x14170F870);

            var animationBindingUUID = ReadUUID(reader);
            var animationName = ClusterDefinition_read_name(reader);

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
            var version = ReadVersion(reader, 1, 0x14170C780);

            var UnknownCount = reader.ReadUInt32();
            for (int i = 0; i < UnknownCount; i++)
            {
                Read_Animation_v5_inner(reader);
            }
        }


        private void Read_Animation_v8_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 2, 0x14170FBF0);

            var animationOverrideName = ClusterDefinition_read_name(reader);
            var animationName = ClusterDefinition_read_name(reader);
            var animationBindingUUID = ReadUUID(reader);

            if(version >= 2)
            {
                var animationSkeletonName = ClusterDefinition_read_name(reader);
            }
        }
        private void Read_Animation_v8(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x14170CE40);

            var UnknownCount = reader.ReadUInt32();
            for (int i = 0; i < UnknownCount; i++)
            {
                Read_Animation_v8_inner(reader);
            }
        }

        private void Read_Animation_v10(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x14170CE50);

            var UnknownCount = reader.ReadUInt32();
            for (int i = 0; i < UnknownCount; i++)
            {
                var innerVersion = ReadVersion(reader, 1, 0x14170FC00);
                var skeletonMapperUUID = ReadUUID(reader);
            }
        }

        private void Read_Animation_v11_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x141205320);

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
            var version = ReadVersion(reader, 1, 0x1411F80D0);

            var UnknownCount = reader.ReadUInt32();
            for (int i = 0; i < UnknownCount; i++)
            {
                ReadString(reader);
                Read_Animation_v11_inner(reader);
            }
        }

        private void Read_Animation(BinaryReader reader)
        {
            var version = ReadVersion(reader, 12, 0x1417056D0);
            var projectDataUUID = ReadUUID(reader);
            Read_RigidBody_post_common(reader);
            var skeletonUUID = ReadUUID(reader);
            var animationBindingUUID = ReadUUID(reader);

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
                var version6_version = ReadVersion(reader, 1, 0x1416F4E40);
                var name = ClusterDefinition_read_name(reader);
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
            var version = ReadVersion(reader, 1, 0x1413A0990);

            var unknownA = reader.ReadUInt32();
            if (unknownA != 0)
            {
               // if (!animationInitialized)
                {
                    Read_Animation(reader);
                }
            }

        }

        private void ClusterDefinition_read_Pose(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1413A0990);
            var unknownA = reader.ReadUInt32();

            if(unknownA != 0)
            {
                // possibly flag to prevent reading version...
                var version_inner = ReadVersion(reader, 1, 0x1417056E0);
            }
        }

        private void ClusterDefinition_read_Char_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 3, 0x1417056F0);

            var speachAlgorithmUUID = ReadUUID(reader);
            var speechCharacterUUID = ReadUUID(reader);

            if(version >= 2)
            {
                var unknown = reader.ReadByte();
            }

            if(version >= 3)
            {
                var version3_version = ReadVersion(reader, 1, 0x1416F4E40);
                var name = ClusterDefinition_read_name(reader);
            }
        }

        private void ClusterDefinition_read_Char(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1413A0990);
            var unknownA = reader.ReadUInt32();

            if (unknownA != 0)
            {
                ClusterDefinition_read_Char_inner(reader);
            }
        }


        private void ClusterDefinition_read_Camera_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 3, 0x141705700);

            reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();

            if(version >= 2)
            {
                var version2_version = ReadVersion(reader, 1, 0x1416F4E40);
                var name = ClusterDefinition_read_name(reader);
            }

            if(version >= 3)
            {
                reader.ReadUInt32();
                reader.ReadUInt32();
            }
        }

        private void ClusterDefinition_read_Camera(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1413A0990);

            var unknownA = reader.ReadUInt32();
            if(unknownA != 0)
            {
                ClusterDefinition_read_Camera_inner(reader);
            }
        }


        private void ClusterDefinition_read_Light_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 5, 0x141705710);

            var unknownA = reader.ReadUInt32();

            var name = ClusterDefinition_read_name(reader);

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

            if (version >= 2)
            {
                reader.ReadByte();
            }

            if(version >= 4)
            {
                var version4_version = ReadVersion(reader, 1, 0x1416F4E40);
                var name2 = ClusterDefinition_read_name(reader);
            }

            if(version >= 5)
            {
                reader.ReadUInt32();
                reader.ReadUInt32();
            }
        }
        private void ClusterDefinition_read_Light(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1413A0990);

            var unknownA = reader.ReadUInt32();
            if(unknownA != 0)
            {
                ClusterDefinition_read_Light_inner(reader);
            }
        }

        private void ClusterDefinition_read_StaticMesh_inner_modelDefinition_inner_inner_v4_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1412224B0);

            reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();
            reader.ReadUInt32();
        }
        private void ClusterDefinition_read_StaticMesh_inner_modelDefinition_inner_inner_v4(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x14121CBA0);

            var unknownCounter = reader.ReadUInt32();
            for (int i = 0; i < unknownCounter; i++)
            {
                ClusterDefinition_read_StaticMesh_inner_modelDefinition_inner_inner_v4_inner(reader);
            }
        }

        private void ClusterDefinition_read_StaticMesh_inner_modelDefinition_inner_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 4, 0x14120E190);

            var materialUUID = ReadUUID(reader);

            if(version >= 4)
            {
                // call
                ClusterDefinition_read_StaticMesh_inner_modelDefinition_inner_inner_v4(reader);
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
        private void ClusterDefinition_read_StaticMesh_inner_modelDefinition_inner(BinaryReader reader)
        {
            var version_inner_inner = ReadVersion(reader, 1, 0x141206410);
            var unknownCount = reader.ReadUInt32();

            for (var i = 0; i < unknownCount; ++i)
            {
                ClusterDefinition_read_StaticMesh_inner_modelDefinition_inner_inner(reader);
            }
        }

        private void ClusterDefinition_read_StaticMesh_inner_modelDefinition(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1413A0990);

            var unknownA = reader.ReadUInt32();
            if (unknownA != 0)
            {
                var version_inner = ReadVersion(reader, 2, 0x1411FD8F0);
                var geometryUUID = ReadUUID(reader);

                ClusterDefinition_read_StaticMesh_inner_modelDefinition_inner(reader);

                if(version_inner < 2)
                {
                    // noop
                }
            }
        }

        private void ClusterDefinition_read_StaticMesh_inner_v3(BinaryReader reader)
        {
            var version = ReadVersion(reader, 5, 0x141706B40);

            var name = ClusterDefinition_read_name(reader);

            // modelDefinition
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
            var version = ReadVersion(reader, 3, 0x141705720);

            if(version >= 3)
            {
                ClusterDefinition_read_StaticMesh_inner_v3(reader);
            }
            else
            {
                var name = ClusterDefinition_read_name(reader);

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
            var version = ReadVersion(reader, 1, 0x1413A0990);

            var unknownA = reader.ReadUInt32();
            if (unknownA != 0)
            {
                ClusterDefinition_read_StaticMesh_inner(reader);
            }
        }

        private void ClusterDefinition_read_RiggedMesh_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 3, 0x141705730);

            if(version >= 3)
            {
                ClusterDefinition_read_StaticMesh_inner_v3(reader);
            }
            else
            {
                var name = ClusterDefinition_read_name(reader);

                // modelDefinition
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
            var version = ReadVersion(reader, 1, 0x1413A0990);

            var unknownA = reader.ReadUInt32();
            if (unknownA != 0)
            {
                ClusterDefinition_read_RiggedMesh_inner(reader);
            }
        }

        private void ClusterDefinition_read_Audio_inner_preinner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x14170C990);

            var unknownA = reader.ReadUInt32();

            var result = unknownA - 2;
            if(result != 0)
            {
                if(result == 1)
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
            var version = ReadVersion(reader, 4, 0x141705740);

            if(version < 3)
            {
                var bankResourceUUID = ReadUUID(reader);
            }

            Read_RigidBody_v6_inner_inner(reader);
            ClusterDefinition_read_Audio_inner_preinner(reader);


            if(version >= 2)
            {
                var version2_version = ReadVersion(reader, 1, 0x1416F4E40);
                var name = ClusterDefinition_read_name(reader);
            }
            if(version >= 4)
            {
                reader.ReadUInt32();
                reader.ReadUInt32();
            }

        }

        private void ClusterDefinition_read_Audio(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1413A0990);

            var unknownA = reader.ReadUInt32();
            if (unknownA != 0)
            {
                ClusterDefinition_read_Audio_inner(reader);
            }
        }

        private void ClusterDefinition_read_Terrain_inner_v2(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x141205310);

            // m128i
            reader.ReadUInt64();
            reader.ReadUInt64();

            // m128i
            reader.ReadUInt64();
            reader.ReadUInt64();
        }

        private void ClusterDefinition_read_Terrain_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 3, 0x141706740);
            var name = ClusterDefinition_read_name(reader);

            var versionB = ReadVersion(reader, 1, 0x14170D340);

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
            var version = ReadVersion(reader, 1, 0x1413A0990);

            var unknownA = reader.ReadUInt32();
            if (unknownA != 0)
            {
                ClusterDefinition_read_Terrain_inner(reader);
            }
        }


        private void ClusterDefinition_read_IKBody(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1413A0990);

            var unknownA = reader.ReadUInt32();
            if (unknownA != 0)
            {
                var version_inner = ReadVersion(reader, 1, 0x141706750);
            }
        }

        private void ClusterDefinition_read_Movement(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1413A0990);

            var unknownA = reader.ReadUInt32();
            if (unknownA != 0)
            {
                var version_inner = ReadVersion(reader, 1, 0x1416F4E40);
                var name = ClusterDefinition_read_name(reader);
            }
        }

        private void ClusterDefinition_read_spawnPoint_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 2, 0x141706760);

            reader.ReadByte();

            if(version >= 2)
            {
                var version_inner = ReadVersion(reader, 1, 0x1416F4E40);
                var name = ClusterDefinition_read_name(reader);
            }
        }

        private void ClusterDefinition_read_spawnPoint(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1413A0990);

            var unknownA = reader.ReadUInt32();
            if (unknownA != 0)
            {
                ClusterDefinition_read_spawnPoint_inner(reader);
            }
        }


        private void ClusterDefinition_reader4(BinaryReader reader)
        {
            var version = ReadVersion(reader, 5, 0x1416F8590);

            ClusterDefinition_reader5(reader);

            
            ClusterDefinition_read_RigidBody(reader);   // rigidBodyDef
            ClusterDefinition_read_Animation(reader);   // animationComponentDef
            ClusterDefinition_read_Pose(reader);        // poseComponentDef
            ClusterDefinition_read_Char(reader);        // charComponentDef
            ClusterDefinition_read_Camera(reader);      // cameraComponentDef
            ClusterDefinition_read_Light(reader);       // lightComponentDef
            ClusterDefinition_read_StaticMesh(reader);  // staticMeshComponentDef
            ClusterDefinition_read_RiggedMesh(reader);  // riggedMeshComponentDef
            ClusterDefinition_read_Audio(reader);       // audioComponentDef
            if (version >= 2)
            {
                ClusterDefinition_read_Terrain(reader); // terrainComponentDef
            }
            if (version >= 3)
            {
                ClusterDefinition_read_IKBody(reader);  // ikBodyComponentDef
            }
            if (version >= 4)
            {
                ClusterDefinition_read_Movement(reader);// movementComponentDef
            }
            if (version >= 5)
            {
                ClusterDefinition_read_spawnPoint(reader); // spawnPointComponentDef
            }

            var name = ClusterDefinition_read_name(reader);
            var valid = reader.ReadByte();
        }

        private void ClusterDefinition_reader3(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1416F31F0);

            var unknownCounter = reader.ReadUInt32();
            for (int i = 0; i < unknownCounter; i++)
            {
                ClusterDefinition_reader4(reader);
            }
        }

        private void ClusterDefinition_reader2(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x14119F1D0);

            // m128 in memory, read as 96 bits (matrix row 0 ?)
            var a = reader.ReadUInt64();
            var b = reader.ReadUInt32();

            // m128 in memory, read as 96 bits (matrix row 1 ?)
            var c = reader.ReadUInt64();
            var d = reader.ReadUInt32();

            // m128 in memory, read as 96 bits (matrix row 2 ?)
            var e = reader.ReadUInt64();
            var f = reader.ReadUInt32();
        }

        private void ClusterDefinition_reader1(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x141198AB0);

            ClusterDefinition_reader2(reader);

            // m128 in memory, read as 96 bits
            var a = reader.ReadUInt64();
            var b = reader.ReadUInt32();
        }

        private void ClusterDefinition_reader0_v2_inner_inner_inner_inner_0(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1411CF7F0);

            var unknownCounter = reader.ReadUInt32();
            for (int i = 0; i < unknownCounter; i++)
            {
                ClusterDefinition_reader0_v2_inner_inner_inner(reader);
            }
        }

        private void ClusterDefinition_reader0_v2_inner_inner_inner_inner_A(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1411CF800);

            var unknownCounter = reader.ReadUInt32();
            for (int i = 0; i < unknownCounter; i++)
            {
                ClusterDefinition_reader0_v2_inner_inner_inner(reader);
            }
        }

        private void ClusterDefinition_reader0_v2_inner_inner_inner_inner_B_10k(BinaryReader reader)
        {
            var version = ReadVersion(reader, 2, 0x1411D4980);
            var prompt = ClusterDefinition_read_name(reader);
            reader.ReadByte();

            if(version < 2)
            {
                var inner_version = ReadVersion(reader, 3, 0x1411E4F00);
                var label = ClusterDefinition_reader0_v2B_inner_inner_inner(reader, inner_version, 3);

                if(inner_version < 3)
                {
                    reader.ReadByte(); // skip byte?
                }
                if(inner_version >= 2)
                {
                    var unknownStr = ClusterDefinition_reader0_v2B_inner_inner_inner(reader, inner_version, 3);
                }
            }
        }

        private void ClusterDefinition_reader0_v2_inner_inner_inner_inner_B(BinaryReader reader, uint typeCode, bool versionAtLeast11)
        {
            // ReadScriptMetadata_Attribute_Payload_MethodC(decompressedStream, typeCode, AttributePayloadVersion >= 11);  ???
            if(ScriptMetadataResource.TypeCodeToNameMap.ContainsKey((int)typeCode))
            {
                Console.WriteLine("Type: " + ScriptMetadataResource.TypeCodeToNameMap[(int)typeCode]);
            }
            else
            {
                Console.WriteLine("Type: " + "Unknown");
            }

            if(typeCode <= 0x1004)
            {
                if(typeCode >= 0x1001)
                {
                    // m128i
                    var item1 = reader.ReadUInt64();
                    var item2 = reader.ReadUInt64();
                    return;
                }

                if(typeCode <= 0x204)
                {
                    // GOTO LABEL_50
                    var value = reader.ReadUInt64();
                    return;
                }

                if(typeCode != 0x208)
                {
                    if(typeCode == 0x400)
                    {
                        var str = ReadString(reader);
                        return;
                    }
                    if(typeCode != 0x800)
                    {
                        if(typeCode - 0x801 <= 5)
                        {
                            if(versionAtLeast11)
                            {
                                var item1 = reader.ReadUInt32();
                                var item2 = reader.ReadUInt32();
                                return;
                            }

                            var item3 = reader.ReadUInt64();
                            return;
                        }
                        // throw error
                        throw new Exception("This shouldn't happen...");
                    }

                    var item4 = reader.ReadUInt64(); //skip 8 bytes?
                    return;
                }

                // LABEL_50:
                var item5 = reader.ReadUInt64();
                return;
            }
            if(typeCode > 0x8104)
            {
                if(typeCode == 0x8108)
                {
                    // GOTO: LABEL_50
                    var val = reader.ReadUInt64();
                    return;
                }
                if(typeCode == 0x10000)
                {
                    ClusterDefinition_reader0_v2_inner_inner_inner_inner_B_10k(reader);
                    return;
                }

                ReadVersion(reader, 2, 0x141160230);
                var val1 = reader.ReadUInt64();
                var val2 = reader.ReadUInt64();
            }
            else
            {
                if(typeCode == 0x8104)
                {
                    if(typeCode - 0x8101 > 1)
                    {
                        throw new Exception("Not suppose to happen..");
                    }

                    // GOTO LABEL_50:
                    var val = reader.ReadUInt64();
                    return;
                }
                if(typeCode > 0x4101)
                {
                    var val = reader.ReadUInt64();
                    return;
                }
                if(typeCode == 0x4101)
                {
                    var val = reader.ReadUInt64();
                    return;
                }
                if(typeCode < 0x2000)
                {
                    throw new Exception("not suppose to happen");
                }
                if(typeCode <= 0x2001)
                {
                    var val = reader.ReadUInt64(); // Skip 8 bytes
                    return;
                }
                if(typeCode > 0x2003)
                {
                    throw new Exception("Not suppose to happen..");
                }

                ReadVersion(reader, 2, 0x141196890);

                var val1 = reader.ReadUInt64();
                var val2 = reader.ReadUInt64();
            }
        }

        private void ClusterDefinition_reader0_v2_inner_inner_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 11, 0x1411C1D70);

            var name = ClusterDefinition_read_name(reader);

            if(version < 6)
            {
                // noop
            }

            var unknownA = reader.ReadUInt32();
            var v17 = unknownA & 0xF0000000;
            var v18 = false;

            if (v17 != 0)
            {
                v18 = (v17 == 0x20000000);
            }
            else
            {
                v18 = ((unknownA >> 29) & 1) > 0;
            }

            if (v18)
            {
                ClusterDefinition_reader0_v2_inner_inner_inner_inner_0(reader);
            }
            else
            {
                var v22 = false;
                if (v17 != 0)
                {
                    v22 = v17 == 0x10000000;
                }
                else 
                {
                    v22 = ((unknownA >> 28) & 1) > 0;
                }

                if (v22)
                {
                    ClusterDefinition_reader0_v2_inner_inner_inner_inner_A(reader);
                }
                else
                {
                    ClusterDefinition_reader0_v2_inner_inner_inner_inner_B(reader, unknownA, version >= 11);
                }
            }
        }

        private void ClusterDefinition_reader0_v2_inner_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 4, 0x1416EE010);
            var name = ClusterDefinition_read_name(reader);

            if (version < 3)
            {
                // noop
            }
            else
            {
                var unknowA = reader.ReadUInt32();
            }

            Console.WriteLine("scriptMetadataResourceUUID = ");
            var scriptMetadataResourceUUID = ReadUUID(reader);
            if (version < 2)
            {
                Console.WriteLine("scriptCompiledBytecodeResourceUUID = ");
                var scriptCompiledBytecodeResourceUUID = ReadUUID(reader);
            }

            var unknownCounter = reader.ReadUInt32();
            for (int i = 0; i < unknownCounter; i++)
            {
                ClusterDefinition_reader0_v2_inner_inner_inner(reader);
            }

            if(version >= 4)
            {
                var version4_version = ReadVersion(reader, 1, 0x1416F4E40);
                var name2 = ClusterDefinition_read_name(reader);
            }

        }

        private void ClusterDefinition_reader0_v2_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1413A0990);

            var unknowA = reader.ReadUInt32();
            if(unknowA != 0)
            {
                ClusterDefinition_reader0_v2_inner_inner(reader);
            }

        }
        private void ClusterDefinition_reader0_v2(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1416E9720);

            var unknownCounter = reader.ReadUInt32();
            for (int i = 0; i < unknownCounter; i++)
            {
                // "value"
                ClusterDefinition_reader0_v2_inner(reader);
            }
        }

        private void ClusterDefinition_reader0_v3(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1416E9710);

            var unknownCounter = reader.ReadUInt32();
            for (int i = 0; i < unknownCounter; i++)
            {
                ClusterDefinition_reader0_v2_inner_inner(reader);
            }
        }

        private void eventRouter_64(BinaryReader reader)
        {
            var version = ReadVersion(reader, 2, 0x14170EFF0);

            // m128i
            reader.ReadUInt64();
            reader.ReadUInt64();

            ClusterDefinition_read_Audio_inner_preinner(reader);

            var version2 = ReadVersion(reader, 2, 0x141196890);
            var unknownB = reader.ReadUInt64();
            var unknownC = reader.ReadUInt64();
            var unknownD = reader.ReadUInt32();
        }

        private void eventRouter_256(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x14170F000);
            var url = ClusterDefinition_read_name(reader);
            reader.ReadUInt32();
        }

        private void eventRouter_gt16(BinaryReader reader)
        {
            var version = ReadVersion(reader, 2, 0x14170F010);
            
            if(version >= 2)
            {
                reader.ReadUInt32();
                reader.ReadUInt32(); // unknownA
                return;
            }

            var str = ReadString(reader);
            var unknownA = reader.ReadUInt32();
        }

        private void eventRouter_16(BinaryReader reader)
        {
            var version = ReadVersion(reader, 2, 0x141160230);
            reader.ReadUInt64();
            reader.ReadUInt64();
        }


        private void eventRouter_c2(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x14170EFC0);
            reader.ReadUInt32();
            reader.ReadUInt32();
        }

        private void eventRouter_cn2(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x14170EFD0);
            reader.ReadUInt32();
        }

        private void eventRouter_cz(BinaryReader reader)
        {
            var versionA = ReadVersion(reader, 1, 0x14170EFB0);
            var versionB = ReadVersion(reader, 2, 0x141196890);

            var unknownA = reader.ReadUInt64();
            var unknownB = reader.ReadUInt64();
            var unknownC = reader.ReadByte();
        }


        private void eventRouter_bz(BinaryReader reader)
        {
            var versionA = ReadVersion(reader, 1, 0x14170EFA0);
            var versionB = ReadVersion(reader, 2, 0x141196890);

            var unknownA = reader.ReadUInt64();
            var unknownB = reader.ReadUInt64();
            var unknownC = reader.ReadUInt32();
            var unknownD = reader.ReadByte();
        }

        private void ClusterDefinition_reader0_eventRouter_inner_inner_inner(BinaryReader reader, uint unknownA)
        {
            if(unknownA > 16)
            {
                if(unknownA == 64)
                {
                    eventRouter_64(reader);
                }
                else if(unknownA == 256)
                {
                    eventRouter_256(reader);
                }
                else
                {
                    eventRouter_gt16(reader);
                }
            }
            else if(unknownA == 16)
            {
                var version16_version = ReadVersion(reader, 1, 0x14170EFE0);

                // "m_uuid"
                eventRouter_16(reader);
            }
            else
            {
                var unknownB = unknownA - 1;
                if(unknownB != 0)
                {
                    var unknownC = unknownB - 1;
                    if(unknownC != 0)
                    {
                        if(unknownC == 2)
                        {
                            eventRouter_c2(reader);
                        }
                        else
                        {
                            eventRouter_cn2(reader);
                        }
                    }
                    else
                    {
                        eventRouter_cz(reader);
                    }
                }
                else
                {
                    eventRouter_bz(reader);
                }
            }
        }


        private void ClusterDefinition_reader0_eventRouter_inner_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1417056B0);

            var unknownA = reader.ReadUInt32();
            var unknownB = reader.ReadUInt16();
            var unknownC = reader.ReadUInt32();
            var unknownD = reader.ReadUInt32();
            var unknownE = reader.ReadUInt16();
            var unknownF = reader.ReadUInt32();

            ClusterDefinition_reader0_eventRouter_inner_inner_inner(reader, unknownF);
        }

        private void ClusterDefinition_reader0_eventRouter_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1416FD570);

            var unknownCounter = reader.ReadUInt32();
            for (int i = 0; i < unknownCounter; i++)
            {
                ClusterDefinition_reader0_eventRouter_inner_inner(reader);
            }
        }
        private void ClusterDefinition_reader0_eventRouter(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1413A0990);

            var unknownA = reader.ReadUInt32();
            if(unknownA != 0)
            {
                ClusterDefinition_reader0_eventRouter_inner(reader);
            }
        }

        private uint ClusterDefinition_reader6(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1416F3200);

            var unknownCounter = reader.ReadUInt32();
            for (int i = 0; i < unknownCounter; i++)
            {
                var x = reader.ReadUInt32();
                Console.WriteLine(x);
            }

            return unknownCounter; // this isn't legit but lets hope it works...
        }

        private void ClusterDefinition_reader0_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1416F3520);

            var unknownA = reader.ReadUInt64();
            var unknownB = reader.ReadUInt64();
            var unknownC = reader.ReadUInt32();

            var unknwonD = unknownC - 1;
            if(unknwonD != 0)
            {
                if(unknwonD == 1)
                {
                    var attachmentBoneName = ClusterDefinition_read_name(reader);
                }
                else
                {
                    var constraintDataUUID = ReadUUID(reader);
                }
            }
        }

        private string ClusterDefinition_reader0_v2B_inner_inner_inner(BinaryReader reader, uint version, int max_version)
        {
            if(version >= max_version)
            {
                return "";
            }

            return ReadString(reader);
        }

        private void ClusterDefinition_reader0_v2B_inner_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1416FDFC0);

            var unknownCounter = reader.ReadUInt32();
            for (int i = 0; i < unknownCounter; i++)
            {
                var version_inner = ReadVersion(reader, 3, 0x141706730);

                // "label"
                ClusterDefinition_reader0_v2B_inner_inner_inner(reader, version_inner, 3);

                if(version_inner >= 2)
                {
                    ClusterDefinition_reader0_v2B_inner_inner_inner(reader, version_inner, 3);
                }
            }
        }

        private void ClusterDefinition_reader0_v2B_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 2, 0x1416F8830);
            var prompt = ClusterDefinition_read_name(reader);
            if(version < 2)
            {
                ClusterDefinition_reader0_v2B_inner_inner(reader);
            }
        }
        private void ClusterDefinition_reader0_v2B(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1416F4530);

            var unknownCounter = reader.ReadUInt32();
            for (int i = 0; i < unknownCounter; i++)
            {
                ClusterDefinition_reader0_v2B_inner(reader);
            }
        }

        private void ClusterDefinition_reader0(BinaryReader reader)
        {
            var version = ReadVersion(reader, 5, 0x1416EDFF0);

            ClusterDefinition_reader1(reader);
            ClusterDefinition_reader3(reader);

            if(version < 3)
            {
                ClusterDefinition_reader0_v2(reader);
            }
            else
            {
                ClusterDefinition_reader0_v3(reader);
            }

            // "eventRouter"
            ClusterDefinition_reader0_eventRouter(reader);

            var reader6_result = ClusterDefinition_reader6(reader);

            // why is this 6 for the d5f248... resource
            var unknownX = reader6_result;
            for (int i = 0; i < unknownX; i++)
            {
                ClusterDefinition_reader0_inner(reader);
            }

            if(version >= 2)
            {
                ClusterDefinition_reader0_v2B(reader);
            }
            if(version >= 4)
            {
                this.Name = ClusterDefinition_read_name(reader);
            }
            if(version >= 5)
            {
                reader.ReadUInt32();
            }
        }

        private void ClusterDefinition_reader_End_inner_joinDef(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1413A0990);

            var unknownA = reader.ReadUInt32();
            if (unknownA != 0)
            {
                ClusterDefinition_reader0_inner(reader);
            }
        }

        private void ClusterDefinition_reader_End_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1416EE000);

            var unknownA = reader.ReadUInt64();
            var unknownB = reader.ReadUInt64();
            var unknownC = reader.ReadUInt32();

            //"jointDefinition"
            ClusterDefinition_reader_End_inner_joinDef(reader);
        }

        private void ClusterDefinition_reader_End(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1416E96F0);

            var unknownCounter = reader.ReadUInt32();
            for (int i = 0; i < unknownCounter; i++)
            {
                ClusterDefinition_reader_End_inner(reader);
            }
        }

        private void ClusterDefinition_reader_Start(BinaryReader reader)
        {
            ResourceVersion = (int)ReadVersion(reader, 1, 0x1416E96E0);
            var UnknownCountB = reader.ReadUInt32();

            for (int i = 0; i < UnknownCountB; i++)
            {
                ClusterDefinition_reader0(reader);
            }
        }

        public string Name { get; set; } = "";

        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var reader = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                var version = ReadVersion(reader, 1, 0x1410E3B70);
                ClusterDefinition_reader_Start(reader);
                ClusterDefinition_reader_End(reader);

                Filename = Name;
                Source = Name;
            }
        }
    }
}
