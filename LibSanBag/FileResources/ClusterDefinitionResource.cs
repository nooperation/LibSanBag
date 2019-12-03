﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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

           //Console.WriteLine($"ReadString: {text}");
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
            for (int i = left.Length - 1; i >= 0; i--)
            {
                sb.AppendFormat("{0:x2}", left[i]);
            }
            for (int i = right.Length - 1; i >= 0; i--)
            {
                sb.AppendFormat("{0:x2}", right[i]);
            }

            var uuid = sb.ToString();
            // Console.WriteLine($"ReadUUID: {uuid}");

            return uuid;
        }

        private string ReadStringVersioned(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1416F4E40);
            var name = ReadString(reader);

            return name;
        }

        private string ReadUUID_B(BinaryReader reader)
        {
            var version = ReadVersion(reader, 2, 0x141160230);

            var item1 = reader.ReadUInt64();
            var item2 = reader.ReadUInt64();

            var left = BitConverter.GetBytes(item1);
            var right = BitConverter.GetBytes(item2);

            StringBuilder sb = new StringBuilder();

            for (int i = left.Length - 1; i >= 0; i--)
            {
                sb.AppendFormat("{0:x2}", left[i]);

                if (i == 4 || i == 2 || i ==0)
                {
                    sb.Append('-');
                }
            }
            for (int i = right.Length - 1; i >= 0; i--)
            {
                sb.AppendFormat("{0:x2}", right[i]);

                if (i == 6)
                {
                    sb.Append('-');
                }
            }

            var uuid = sb.ToString();
           // Console.WriteLine($"ReadUUID_B: {uuid}");

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


        private T ReadComponent<T>(BinaryReader reader, Func<BinaryReader, T> func)
        {
            var version = ReadVersion(reader, 1, 0x1413A0990);

            var unknownA = reader.ReadUInt32();
            if (unknownA != 0)
            {
                return func(reader);
            }

            return default(T);
        }

        private void ReadComponent(BinaryReader reader, Action<BinaryReader> func)
        {
            var version = ReadVersion(reader, 1, 0x1413A0990);

            var unknownA = reader.ReadUInt32();
            if (unknownA != 0)
            {
                func(reader);
            }
        }

        private void ClusterDefinition_reader5(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1411A0F00);

            // this is all just a guess
            var first = new List<float>()
            {
                reader.ReadSingle(),
                reader.ReadSingle(),
                reader.ReadSingle(),
                reader.ReadSingle()
            };
            
            var second = new List<float>()
            {
                reader.ReadSingle(),
                reader.ReadSingle(),
                reader.ReadSingle(),
                0.0f
            };

            //Console.WriteLine($"{string.Join(",", first)} | {string.Join(",", second)}");

            return;
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

            var type = reader.ReadUInt32();

            // var localOffset = 
            ClusterDefinition_reader5(reader);

            if (version >= 2)
            {
                var isSticky = reader.ReadByte();
            }
            if (version >= 3)
            {
                var baseDefinition = ReadStringVersioned(reader);
            }
            if (version >= 4)
            {
                var aimAtCursor = reader.ReadByte();
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

            var enabled = reader.ReadByte();
            var loudnessOffset = reader.ReadInt32();
            var pitchRange = reader.ReadInt32();

            // sounds...
            Read_RigidBody_v6_inner_inner(reader);
        }

        private void Read_RigidBody_v6(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x141203830);

            var UnknownCount = reader.ReadUInt32();
            for (int i = 0; i < UnknownCount; i++)
            {
                var data = reader.ReadUInt32();
                Read_RigidBody_v6_inner(reader);
            }
        }

        private void Read_RigidBody_v7_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 2, 0x14170FBD0);

            var type = reader.ReadUInt32();
            
            // var localOffset = 
            ClusterDefinition_reader5(reader);

            if (version >= 2)
            {
                var baseDefinition = ReadStringVersioned(reader);
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

        private void Read_RigidBody_v10_inner(BinaryReader reader)
        {
            var version_inner_a = ReadVersion(reader, 1, 0x14170FBE0);

            // localOffset = 
            ClusterDefinition_reader5(reader);

            var baseDefinition = ReadStringVersioned(reader);
        }

        private void Read_RigidBody_v10(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x14170CCC0);

            var UnknownCount = reader.ReadUInt32();
            for (int i = 0; i < UnknownCount; i++)
            {
                Read_RigidBody_v10_inner(reader);
            }
        }

        private void Read_RigidBody(BinaryReader reader)
        {
            var version = ReadVersion(reader, 13, 0x1417056C0);

            // bodyResourceHandle
            Read_RigidBody_Init(reader);

            // bodyCinfo ?
            var bodyCinfoLength = reader.ReadUInt32();
            var bodyCinfo = reader.ReadBytes((int)bodyCinfoLength);

            // materials?
            Read_RigidBody_common(reader);

            // shape?
            Read_RigidBody_post_common(reader);

            if (version >= 2)
            {
                var audioMaterial = ReadUUID(reader);
            }
            if (version >= 3)
            {
                var canGrab = reader.ReadByte();
            }
            if (version >= 4)
            {
                // grabPointDefinitions
                Read_RigidBody_v4(reader);
            }
            if (version >= 6)
            {
                // audioResourcePoolSounds
                Read_RigidBody_v6(reader);
            }
            if (version >= 7)
            {
                // sitPointDefinitions
                Read_RigidBody_v7(reader);
            }
            if (version >= 8)
            {
                var canRide = reader.ReadByte();
            }
            if (version >= 9)
            {
                var baseDefinition = ReadStringVersioned(reader);
            }
            if (version >= 10)
            {
                // selectionBeamPointDefinitions
                Read_RigidBody_v10(reader);
            }
            if (version >= 12)
            {
                var editInstanceId = reader.ReadUInt32();
                var editLinkId = reader.ReadUInt32();
            }
            if (version >= 13)
            {
                var collisionFilterOverride = reader.ReadUInt32();
            }
        }

        private void Read_AnimationComponent_v5_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 3, 0x14170F870);

            var animationBindingUUID = ReadUUID(reader);
            var animationName = ReadString(reader);

            var playbackSpeed = reader.ReadUInt32();
            var startTime = reader.ReadUInt32();
            var playbackMode = reader.ReadUInt32();

            if (version == 2)
            {
                var animationBindingRemapIndex = reader.ReadUInt32();
            }
        }
        private void Read_AnimationComponent_v5(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x14170C780);

            var UnknownCount = reader.ReadUInt32();
            for (int i = 0; i < UnknownCount; i++)
            {
                Read_AnimationComponent_v5_inner(reader);
            }
        }


        private void Read_AnimationComponent_v8_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 2, 0x14170FBF0);

            var animationOverrideName = ReadString(reader);
            var animationName = ReadString(reader);
            var animationBindingUUID = ReadUUID(reader);

            if (version >= 2)
            {
                var animationSkeletonName = ReadString(reader);
            }
        }
        private void Read_AnimationComponent_v8(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x14170CE40);

            var UnknownCount = reader.ReadUInt32();
            for (int i = 0; i < UnknownCount; i++)
            {
                Read_AnimationComponent_v8_inner(reader);
            }
        }

        private void Read_AnimationComponent_v10(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x14170CE50);

            var UnknownCount = reader.ReadUInt32();
            for (int i = 0; i < UnknownCount; i++)
            {
                var innerVersion = ReadVersion(reader, 1, 0x14170FC00);
                var skeletonMapperUUID = ReadUUID(reader);
            }
        }

        private void Read_AnimationComponent_v11_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x141205320);

            // m128i
            // translation
            reader.ReadInt64();
            reader.ReadInt64();

            // m128i
            // quaternion
            reader.ReadInt64();
            reader.ReadInt64();

            // m128i
            // scale
            reader.ReadInt64();
            reader.ReadInt64();
        }

        private void Read_AnimationComponent_v11(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1411F80D0);

            var UnknownCount = reader.ReadUInt32();
            for (int i = 0; i < UnknownCount; i++)
            {
                var data = ReadString(reader);
                Read_AnimationComponent_v11_inner(reader);
            }
        }

        private void Read_AnimationComponent(BinaryReader reader)
        {
            var version = ReadVersion(reader, 12, 0x1417056D0);
            var projectDataUUID = ReadUUID(reader);
            Read_RigidBody_post_common(reader);
            var skeletonUUID = ReadUUID(reader);
            var animationBindingUUID = ReadUUID(reader);

            var creationMode = reader.ReadUInt32();
            var clientOnly = reader.ReadByte();

            if (version >= 2)
            {
                var playbackSpeed = reader.ReadUInt32();
                var startTime = reader.ReadUInt32();
            }

            if (version >= 3)
            {
                var playbackMode = reader.ReadUInt32();
                var beginOnLoad = reader.ReadByte();
            }

            if (version < 4)
            {
                // default animation stuff
            }
            else
            {
                // animationNodes
                Read_AnimationComponent_v5(reader);
            }

            if ((version - 5) <= 2)
            {
                var animationBindingRemapIndex = reader.ReadUInt32();
            }

            if (version >= 6)
            {
                var baseDefiniton = ReadStringVersioned(reader);
            }
            if (version >= 7)
            {
                var scale = reader.ReadUInt32();
            }
            if (version >= 8)
            {
                // animationOverrides
                Read_AnimationComponent_v8(reader);
            }
            if (version >= 9)
            {
                var editInstanceId = reader.ReadUInt32();
                var editLinkId = reader.ReadUInt32();
            }
            if (version >= 10)
            {
                // animationSkeletonMappers
                Read_AnimationComponent_v10(reader);
            }

            if (version == 11)
            {
                // sgOffsetTransformsMap
                Read_AnimationComponent_v11(reader);
            }
        }

        private void Read_PoseComponent(BinaryReader reader)
        {
            var version_inner = ReadVersion(reader, 1, 0x1417056E0);
        }
   
        private void Read_CharComponent(BinaryReader reader)
        {
            var version = ReadVersion(reader, 3, 0x1417056F0);

            var speachAlgorithmUUID = ReadUUID(reader);
            var speechCharacterUUID = ReadUUID(reader);

            if (version >= 2)
            {
                var hadsRigged = reader.ReadByte();
            }

            if (version >= 3)
            {
                var baseDefinition = ReadStringVersioned(reader);
            }
        }

        private void Read_CameraComponent(BinaryReader reader)
        {
            var version = ReadVersion(reader, 3, 0x141705700);

            var nearPlane = reader.ReadUInt32();
            var farPlane = reader.ReadUInt32();
            var diagonalFovRadius = reader.ReadUInt32();

            if (version >= 2)
            {
                var baseDefinition = ReadStringVersioned(reader);
            }

            if (version >= 3)
            {
                var editInstanceId = reader.ReadUInt32();
                var editLinkId = reader.ReadUInt32();
            }
        }

        private void Read_LightComponent(BinaryReader reader)
        {
            var version = ReadVersion(reader, 5, 0x141705710);

            var lightType = reader.ReadUInt32();

            var name = ReadString(reader);

            List<float> rgbIntensity = new List<float>()
            {
                reader.ReadSingle(),
                reader.ReadSingle(),
                reader.ReadSingle(),
                reader.ReadSingle(),
            };

            var range = reader.ReadUInt32();
            var isShadowCaster = reader.ReadByte();

            if (version >= 3)
            {
                var enableDynamicCasters = reader.ReadByte();
            }

            var spot_angle = reader.ReadUInt32();
            var spot_angularFalloff = reader.ReadUInt32();
            var spot_nearClip = reader.ReadUInt32();

            if (version >= 2)
            {
                var isStatic =reader.ReadByte();
            }

            if (version >= 4)
            {
                var baseDefinition = ReadStringVersioned(reader);
            }

            if (version >= 5)
            {
                var editInstanceId = reader.ReadUInt32();
                var editLinkId = reader.ReadUInt32();
            }
        }

        private void Read_ModelDefinition_inner_inner_v4_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1412224B0);

            var indexStart = reader.ReadUInt32();
            var indexCount = reader.ReadUInt32();
            var errorLower = reader.ReadUInt32();
            var errorUpper = reader.ReadUInt32();
        }

        private void Read_ModelDefinition_inner_inner_v4(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x14121CBA0);

            var unknownCounter = reader.ReadUInt32();
            for (int i = 0; i < unknownCounter; i++)
            {
                Read_ModelDefinition_inner_inner_v4_inner(reader);
            }
        }

        private void Read_ModelDefinition_inner_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 4, 0x14120E190);

            var materialUUID = ReadUUID(reader);

            if (version >= 4)
            {
                // lods
                Read_ModelDefinition_inner_inner_v4(reader);
            }
            else
            {
                var indexStart = reader.ReadUInt32();
                var indexCount = reader.ReadUInt32();
            }

            if (version < 2)
            {
                // noop
                // var flags = 0;
            }
            else
            {
                var flags = reader.ReadUInt64();
            }

            if (version < 3)
            {
                // noop
                // var surfaceArea = 1315859240;
            }
            else
            {
                var surfaceArea = reader.ReadUInt32();
            }
        }

        private void Read_ModelDefinition_inner(BinaryReader reader)
        {
            var version_inner_inner = ReadVersion(reader, 1, 0x141206410);

            var unknownCount = reader.ReadUInt32();
            for (var i = 0; i < unknownCount; ++i)
            {
                Read_ModelDefinition_inner_inner(reader);
            }
        }

        private void Read_ModelDefinition(BinaryReader reader)
        {
            var version_inner = ReadVersion(reader, 2, 0x1411FD8F0);

            var geometryUUID = ReadUUID(reader);

            // parts
            Read_ModelDefinition_inner(reader);

            if (version_inner < 2)
            {
                // noop
            }
        }

        private void Read_StaticMeshComponent_inner_v3(BinaryReader reader)
        {
            var version = ReadVersion(reader, 5, 0x141706B40);
            var name = ReadString(reader);

            // modelDefinition
            ReadComponent(reader, Read_ModelDefinition);

            var scale = reader.ReadUInt32();

            if (version >= 2)
            {
                var maxRenderDistance = reader.ReadUInt32();
            }
            if (version >= 3)
            {
                var inScriptable = reader.ReadByte();
            }
            if (version >= 4)
            {
                var isVisible = reader.ReadByte();
            }
            if (version >= 5)
            {
                var editInstanceId = reader.ReadUInt32();
                var editLinkId = reader.ReadUInt32();
            }
        }
        private void Read_StaticMeshComponent(BinaryReader reader)
        {
            var version = ReadVersion(reader, 3, 0x141705720);

            if (version >= 3)
            {
                Read_StaticMeshComponent_inner_v3(reader);
            }
            else
            {
                var name = ReadString(reader);

                // modelDefinition
                ReadComponent(reader, Read_ModelDefinition);

                // skip?? TODO: Remove me most likely
                var shadowCaster = reader.ReadByte();

                if (version >= 2)
                {
                    var scale = reader.ReadUInt32();
                }
            }
        }

        private void Read_RiggedMeshComponent(BinaryReader reader)
        {
            var version = ReadVersion(reader, 3, 0x141705730);

            if (version >= 3)
            {
                Read_StaticMeshComponent_inner_v3(reader);
            }
            else
            {
                var name = ReadString(reader);

                // modelDefinition
                ReadComponent(reader, Read_ModelDefinition);

                // yeah i don't know. maybe? i think we skip this byte though
                var shadowCaster = reader.ReadByte();

                if (version >= 2)
                {
                    var scale = reader.ReadUInt32();
                }
            }
        }

        // read_audiocomponent_shape ?
        private void Read_AudioComponent_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x14170C990);

            var type = reader.ReadUInt32();

            if(type == 2)
            {
                var sphereRadius = reader.ReadUInt32();
            }
            else if (type == 3)
            {
                //var aabaa = 
                Read_AABB(reader);
            }
        }

        private void Read_AudioComponent(BinaryReader reader)
        {
            var version = ReadVersion(reader, 4, 0x141705740);

            if (version < 3)
            {
                var bankResourceUUID = ReadUUID(reader);
            }

            // soundResources
            Read_RigidBody_v6_inner_inner(reader);

            // shape
            Read_AudioComponent_inner(reader);

            if (version >= 2)
            {
                var baseDefinition = ReadStringVersioned(reader);
            }
            if (version >= 4)
            {
                var editInstanceId = reader.ReadUInt32();
                var editLinkId = reader.ReadUInt32();
            }

        }

        private void Read_AABB(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x141205310);

            // min =m128i xyzw
            reader.ReadUInt64();
            reader.ReadUInt64();

            // max = m128i
            reader.ReadUInt64();
            reader.ReadUInt64();
        }

        private void Read_TerrainComponent_RuntimeData_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 3, 0x141710770);

            if (version >= 2)
            {
                // skip 4 bytes...
                var unused = reader.ReadUInt32();
            }

            // m128i - voxelTextureData?
            var a = reader.ReadUInt64();
            var b = reader.ReadUInt64();

            var versionb = ReadVersion(reader, 1, 0x141710930);
            var VoxelTextureData = ReadUUID(reader);

            var versionc = ReadVersion(reader, 2, 0x141710940);
            var HeightMap = ReadUUID(reader);
            var MaterialMap = ReadUUID(reader);

            if(versionc >= 2)
            {
                var MaterialMapB = ReadUUID(reader);
            }

            // might not be readuuid...
            var runtimeTextureData = ReadUUID(reader);

            if(version >= 3)
            {
                // skip 16 bytes...
                reader.ReadUInt64(); // skip?
                reader.ReadUInt64(); // skip?
            }
        }

        private void Read_TerrainComponent_RuntimeData(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x14170FC10);

            var count = reader.ReadUInt32();
            for (int i = 0; i < count; i++)
            {
                Read_TerrainComponent_RuntimeData_inner(reader);
            }
        }

        private void Read_TerrainComponent(BinaryReader reader)
        {
            var version = ReadVersion(reader, 3, 0x141706740);
            var name = ReadString(reader);
            var versionB = ReadVersion(reader, 1, 0x14170D340);

            // runtimeData = ??
            Read_TerrainComponent_RuntimeData(reader);

            if (version >= 2)
            {
                // var aabb = 
                Read_AABB(reader);
            }

            if (version >= 3)
            {
                var audioMaterialIndex = reader.ReadUInt64();
            }
        }

        private void Read_IKBodyComponent(BinaryReader reader)
        {
            var version_inner = ReadVersion(reader, 1, 0x141706750);
        }

        private void Read_MovementComponent(BinaryReader reader)
        {
            var name = ReadStringVersioned(reader);
        }

        private void Read_SpawnPointComponent(BinaryReader reader)
        {
            var version = ReadVersion(reader, 2, 0x141706760);
            var initialSpawnPoint = reader.ReadByte();

            if (version >= 2)
            {
                var baseDefinition = ReadStringVersioned(reader);
            }
        }

        private void ClusterDefinition_reader4(BinaryReader reader)
        {
            var version = ReadVersion(reader, 5, 0x1416F8590);

            // entityToObject ?
            ClusterDefinition_reader5(reader);

            ReadComponent(reader, Read_RigidBody);                // rigidBodyDef
            ReadComponent(reader, Read_AnimationComponent);       // animationComponentDef
            ReadComponent(reader, Read_PoseComponent);            // poseComponentDef
            ReadComponent(reader, Read_CharComponent);            // charComponentDef
            ReadComponent(reader, Read_CameraComponent);          // cameraComponentDef
            ReadComponent(reader, Read_LightComponent);           // lightComponentDef
            ReadComponent(reader, Read_StaticMeshComponent);      // staticMeshComponentDef
            ReadComponent(reader, Read_RiggedMeshComponent);      // riggedMeshComponentDef
            ReadComponent(reader, Read_AudioComponent);           // audioComponentDef

            if (version >= 2)
            {
                ReadComponent(reader, Read_TerrainComponent);     // terrainComponentDef
            }
            if (version >= 3)
            {
                ReadComponent(reader, Read_IKBodyComponent);      // ikBodyComponentDef
            }
            if (version >= 4)
            {
                ReadComponent(reader, Read_MovementComponent);    // movementComponentDef
            }
            if (version >= 5)
            {
                ReadComponent(reader, Read_SpawnPointComponent);  // spawnPointComponentDef
            }

            var name = ReadString(reader);
            var fixedInScene = reader.ReadByte();
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
            //var a = reader.ReadUInt64();
            //var b = reader.ReadUInt32();

            List<float> second = new List<float>()
            {
                reader.ReadSingle(),
                reader.ReadSingle(),
                reader.ReadSingle(),
                0,
            };
            //Console.WriteLine($"C0: <{string.Join(",", second)}>");

            // m128 in memory, read as 96 bits (matrix row 1 ?)
            //var c = reader.ReadUInt64();
           // var d = reader.ReadUInt32();

            second = new List<float>()
            {
                reader.ReadSingle(),
                reader.ReadSingle(),
                reader.ReadSingle(),
                0,
            };
            //Console.WriteLine($"C1: <{string.Join(",", second)}>");

            // m128 in memory, read as 96 bits (matrix row 2 ?)
           // var e = reader.ReadUInt64();
           // var f = reader.ReadUInt32();

            second = new List<float>()
            {
                reader.ReadSingle(),
                reader.ReadSingle(),
                reader.ReadSingle(),
                0,
            };
            //Console.WriteLine($"C2: <{string.Join(",", second)}>");
        }

        private void ClusterDefinition_reader1(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x141198AB0);

            ClusterDefinition_reader2(reader);

            // m128 in memory, read as 96 bits
            //var a = reader.ReadUInt64();
            //var b = reader.ReadUInt32();

            List<float> second = new List<float>()
            {
                reader.ReadSingle(),
                reader.ReadSingle(),
                reader.ReadSingle(),
                0,
            };
            //Console.WriteLine($"D: <{string.Join(",", second)}>");
        }

        private void Read_ScriptComponent_values_a(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1411CF7F0);

            var propertyCount = reader.ReadUInt32();
            for (int i = 0; i < propertyCount; i++)
            {
                Read_ScriptComponent_parameter(reader);
            }
        }

        private void Read_ScriptComponent_values_b(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1411CF800);

            var propertyCount = reader.ReadUInt32();
            for (int i = 0; i < propertyCount; i++)
            {
                Read_ScriptComponent_parameter(reader);
            }
        }

        class InteractionResult
        {
            public string Prompt { get; set; }
            public string Label { get; set; }
            public bool Enabled { get; set; }
            public string Key { get; set; }

            public override string ToString()
            {
                return $"Prompt: {Prompt} | Label: {Label} | Enabled={Enabled} | Key: {Key}";
            }
        }

        private InteractionResult Read_ScriptComponent_Interaction(BinaryReader reader)
        {
            var version = ReadVersion(reader, 2, 0x1411D4980);
            var prompt = ReadString(reader);
            var enabled = reader.ReadByte();

            var label = string.Empty;
            var key = string.Empty;
            if (version < 2)
            {
                var inner_version = ReadVersion(reader, 3, 0x1411E4F00);
                label = ClusterDefinition_reader0_v2B_inner_inner_inner(reader, inner_version, 3);

                if (inner_version < 3)
                {
                    var enabled_old = reader.ReadByte(); // skip byte?
                }
                if (inner_version >= 2)
                {
                    key = ClusterDefinition_reader0_v2B_inner_inner_inner(reader, inner_version, 3);
                }
            }

            return new InteractionResult()
            {
                Label = label,
                Prompt = prompt,
                Key = key
            };
        }

        public struct ScriptTypeCodes
        {
            public const int System_Boolean = 0x4101;
            public const int System_SByte = 0x101;
            public const int System_Byte = 0x8101;
            public const int System_Int16 = 0x102;
            public const int System_UInt16 = 0x8102;
            public const int System_Int32 = 0x104;
            public const int System_UInt32 = 0x8104;
            public const int System_Int64 = 0x108;
            public const int System_UInt64 = 0x8108;
            public const int System_Single = 0x204;
            public const int System_Double = 0x208;
            public const int System_String = 0x400;
            public const int System_Object = 0x800;
            public const int Sansar_Simulation_AnimationComponent = 0x801;
            public const int Sansar_Simulation_RigidBodyComponent = 0x802;
            public const int Sansar_Simulation_AudioComponent = 0x803;
            public const int Sansar_Simulation_LightComponent = 0x804;
            public const int Sansar_Simulation_MeshComponent = 0x805;
            public const int Sansar_Simulation_CameraComponent = 0x806;
            public const int Sansar_Simulation_UnknownResourceA = 0x2000;
            public const int Sansar_Simulation_UnknownResourceB = 0x2001;
            public const int Sansar_Simulation_ClusterResource = 0x2002;
            public const int Sansar_Simulation_SoundResource = 0x2003;
            public const int Sansar_Simulation_Interaction = 0x10000;
            public const int Sansar_Simulation_QuestDefinition = 0x20000;
            public const int Sansar_Simulation_ObjectiveDefinition = 0x20001;
            public const int Sansar_Simulation_QuestCharacter = 0x20002;
            public const int Mono_Simd_Vector4f = 0x1001;
            public const int Sansar_Vector = 0x1002;
            public const int Sansar_Quaternion = 0x1003;
            public const int Sansar_Color = 0x1004;
        }

        private object Read_ScriptComponent_value(BinaryReader reader, uint scriptTypeCode, bool versionAtLeast11)
        {
            object result = null;
            switch (scriptTypeCode)
            {
                case ScriptTypeCodes.System_SByte: //0x101
                    result = (char)reader.ReadInt64();
                    break;
                case ScriptTypeCodes.System_Int16: //0x102
                    result = (short)reader.ReadInt64();
                    break;
                case ScriptTypeCodes.System_Int32: //0x104
                    result = (int)reader.ReadInt64();
                    break;
                case ScriptTypeCodes.System_Int64: //0x108
                    result = reader.ReadInt64();
                    break;
                case ScriptTypeCodes.System_Single: //0x204
                    result = reader.ReadDouble();
                    break;
                case ScriptTypeCodes.System_Double: //0x208
                    result = reader.ReadDouble();
                    break;
                case ScriptTypeCodes.System_String: //0x400
                    result = ReadString(reader);
                    break;
                case ScriptTypeCodes.System_Object: //0x800
                    result = reader.ReadUInt64(); //skip 8 bytes
                    break;
                case ScriptTypeCodes.Sansar_Simulation_AnimationComponent: //0x801
                case ScriptTypeCodes.Sansar_Simulation_RigidBodyComponent: //0x802
                case ScriptTypeCodes.Sansar_Simulation_AudioComponent: //0x803
                case ScriptTypeCodes.Sansar_Simulation_LightComponent: //0x804
                case ScriptTypeCodes.Sansar_Simulation_MeshComponent: //0x805
                case ScriptTypeCodes.Sansar_Simulation_CameraComponent: //0x806
                {
                    if (versionAtLeast11)
                    {
                        var item1 = reader.ReadUInt32();
                        var item2 = reader.ReadUInt32();
                        result = $"{item1} | {item2}";
                    }
                    else
                    {
                        result = reader.ReadUInt64();
                    }
                    break;
                }
                case ScriptTypeCodes.Mono_Simd_Vector4f: //0x1001
                case ScriptTypeCodes.Sansar_Vector: //0x1002
                case ScriptTypeCodes.Sansar_Quaternion: //0x1003
                case ScriptTypeCodes.Sansar_Color: // = 0x1004
                {
                    List<float> second = new List<float>()
                    {
                        reader.ReadSingle(),
                        reader.ReadSingle(),
                        reader.ReadSingle(),
                        reader.ReadSingle(),
                    };
                    result = $"<{string.Join(",", second)}>";
                    break;
                }
                case ScriptTypeCodes.Sansar_Simulation_UnknownResourceA: //0x2000
                case ScriptTypeCodes.Sansar_Simulation_UnknownResourceB: //0x2001
                {
                    result = reader.ReadUInt64(); // Skip 8 bytes
                    break;
                }
                case ScriptTypeCodes.Sansar_Simulation_ClusterResource: //0x2002
                case ScriptTypeCodes.Sansar_Simulation_SoundResource: //0x2003
                {
                    result = ReadUUID(reader);
                    break;
                }
                case ScriptTypeCodes.System_Boolean: //0x4101
                    result = (reader.ReadUInt64() != 0);
                    break;
                case ScriptTypeCodes.System_Byte: //0x8101
                    result = (byte)reader.ReadUInt64();
                    break;
                case ScriptTypeCodes.System_UInt16: //0x8102
                    result = (ushort)reader.ReadUInt64();
                    break;
                case ScriptTypeCodes.System_UInt32: //0x8104
                    result = (uint)reader.ReadUInt64();
                    break;
                case ScriptTypeCodes.System_UInt64: //0x8108
                    result = reader.ReadUInt64();
                    break;
                case ScriptTypeCodes.Sansar_Simulation_Interaction: //0x10000
                    result = Read_ScriptComponent_Interaction(reader);
                    break;
                case ScriptTypeCodes.Sansar_Simulation_QuestDefinition: //0x20000
                    result = ReadUUID_B(reader);
                    break;
                case ScriptTypeCodes.Sansar_Simulation_ObjectiveDefinition: //0x20001
                    result = ReadUUID_B(reader);
                    break;
                case ScriptTypeCodes.Sansar_Simulation_QuestCharacter: //0x20002
                    result = ReadUUID_B(reader);
                    break;
                default:
                    Console.WriteLine($"Unknown type code: {scriptTypeCode:x}");
                    break;
            }

            return result;
        }

        private void Read_ScriptComponent_parameter(BinaryReader reader)
        {
            var version = ReadVersion(reader, 11, 0x1411C1D70);
            var name = ReadString(reader);

            if (version < 6)
            {
                // noop
            }

            var type = reader.ReadUInt32();
            if ((type & (1 << 29)) != 0)
            {
                Read_ScriptComponent_values_a(reader);
            }
            else if ((type & (1 << 28)) != 0)
            {
                Read_ScriptComponent_values_b(reader);
            }
            else
            {
                Read_ScriptComponent_value(reader, type, version >= 11);
            }
        }

        private void Read_ScriptComponent(BinaryReader reader)
        {
            var version = ReadVersion(reader, 4, 0x1416EE010);
            var name = ReadString(reader);

            if (version < 3)
            {
                // noop
            }
            else
            {
                var scriptType = reader.ReadUInt32();
            }

            var scriptMetadataResourceUUID = ReadUUID(reader);
            if (version < 2)
            {
                var scriptCompiledBytecodeResourceUUID = ReadUUID(reader);
            }

            var paramCount = reader.ReadUInt32();
            for (int i = 0; i < paramCount; i++)
            {
                // parameter -> value
                Read_ScriptComponent_parameter(reader);
            }

            if (version >= 4)
            {
                var baseDefinition = ReadStringVersioned(reader);
            }
        }

        private void ClusterDefinition_reader0_v2(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1416E9720);

            var unknownCounter = reader.ReadUInt32();
            for (int i = 0; i < unknownCounter; i++)
            {
                // "value"
                ReadComponent(reader, Read_ScriptComponent);
            }
        }

        private void ClusterDefinition_reader0_v3(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1416E9710);

            var unknownCounter = reader.ReadUInt32();
            for (int i = 0; i < unknownCounter; i++)
            {
                Read_ScriptComponent(reader);
            }
        }

        private void eventRouter_64(BinaryReader reader)
        {
            var version = ReadVersion(reader, 2, 0x14170EFF0);

            var  location = new List<int>()
            {
                reader.ReadInt32(), // X
                reader.ReadInt32(), // Y
                reader.ReadInt32(), // Z
                reader.ReadInt32(), // W
            };
            // shape = ??? 

            Read_AudioComponent_inner(reader);

            var soundResourceId = ReadUUID(reader);
            var loudness = reader.ReadUInt32();
        }

        private void eventRouter_256(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x14170F000);

            var url = ReadString(reader);
            var loudness = reader.ReadUInt32();
        }

        private void eventRouter_gt16(BinaryReader reader)
        {
            var version = ReadVersion(reader, 2, 0x14170F010);

            if (version >= 2)
            {
                var streamChannel = reader.ReadUInt32();
            }
            else
            {
                var url = ReadString(reader);
            }

            var loudness = reader.ReadUInt32();
        }

        private void eventRouter_16(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x14170EFE0);

            var m_uuid = ReadUUID_B(reader);
        }

        private void eventRouter_c2(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x14170EFC0);
            var streamTagHash = reader.ReadUInt32();
            var loudness = reader.ReadUInt32();
        }

        private void eventRouter_cn2(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x14170EFD0);
            var streamTagHash = reader.ReadUInt32();
        }

        private void eventRouter_cz(BinaryReader reader)
        {
            var versionA = ReadVersion(reader, 1, 0x14170EFB0);

            var soundResourceId = ReadUUID(reader);
            var immediate = reader.ReadByte();
        }


        private void eventRouter_bz(BinaryReader reader)
        {
            var versionA = ReadVersion(reader, 1, 0x14170EFA0);

            var soundResourceId = ReadUUID(reader);
            var loudness = reader.ReadUInt32();
            var loop = reader.ReadByte();
        }

        private void Read_EventRouter_inner_inner(BinaryReader reader, uint unknownA)
        {
            Console.WriteLine("UnknownA = " + unknownA);

            if (unknownA == 64)
            {
                eventRouter_64(reader);
            }
            else if (unknownA == 256)
            {
                eventRouter_256(reader);
            }
            else if (unknownA == 16)
            {
                eventRouter_16(reader);
            }
            else if (unknownA == 1)
            {
                eventRouter_bz(reader);
            }
            else if (unknownA == 2)
            {
                eventRouter_cz(reader);
            }
            else if (unknownA == 4)
            {
                eventRouter_c2(reader);
            }
            else if (unknownA > 16)
            {
                eventRouter_gt16(reader);
            }
            else if(unknownA < 16)
            {
                eventRouter_cn2(reader);
            }
        }

        private void Read_EventRouter_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1417056B0);

            var sourceEmitter = reader.ReadUInt32();
            var sourceEntityIndex = reader.ReadUInt16();
            var sourceEvent = reader.ReadUInt32();
            var targetReceiver = reader.ReadUInt32();
            var targetEntityIndex = reader.ReadUInt16();
            var targetEvent = reader.ReadUInt32();

            // targetEventParams
            Read_EventRouter_inner_inner(reader, targetEvent);
        }

        private void Read_EventRouter(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1416FD570);

            var unknownCounter = reader.ReadUInt32();
            for (int i = 0; i < unknownCounter; i++)
            {
                Read_EventRouter_inner(reader);
            }
        }

        private uint ClusterDefinition_reader6(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1416F3200);

            var unknownCounter = reader.ReadUInt32();
            for (int i = 0; i < unknownCounter; i++)
            {
                var x = reader.ReadUInt32();
            }

            return unknownCounter; // this isn't legit but lets hope it works...
        }

        private void Read_JointDefinition(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1416F3520);

            var unknownA = reader.ReadUInt64(); // entityA
            var unknownB = reader.ReadUInt64(); // entityB
            var unknownC = reader.ReadUInt32(); // type

            var unknwonD = unknownC - 1;
            if (unknwonD != 0)
            {
                if (unknwonD == 1)
                {
                    var attachmentBoneName = ReadString(reader);
                }
                else
                {
                    Console.WriteLine("ConstraintData");
                    var constraintDataUUID = ReadUUID(reader);
                }
            }
        }

        private string ClusterDefinition_reader0_v2B_inner_inner_inner(BinaryReader reader, uint version, int max_version)
        {
            if (version >= max_version)
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

                // label
                ClusterDefinition_reader0_v2B_inner_inner_inner(reader, version_inner, 3);

                if (version_inner >= 2)
                {
                    // key
                    ClusterDefinition_reader0_v2B_inner_inner_inner(reader, version_inner, 3);
                }
            }
        }

        private void ClusterDefinition_reader0_v2B_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 2, 0x1416F8830);
            var prompt = ReadString(reader);

            if (version < 2)
            {
                // actions?
                ClusterDefinition_reader0_v2B_inner_inner(reader);
            }
        }
        private void ClusterDefinition_reader0_v2B(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1416F4530);

            var unknownCounter = reader.ReadUInt32(); // count
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

            if (version < 3)
            {
                ClusterDefinition_reader0_v2(reader);
            }
            else
            {
                ClusterDefinition_reader0_v3(reader);
            }

            // "eventRouter"
            ReadComponent(reader, Read_EventRouter);

            var unknownX = ClusterDefinition_reader6(reader);
            for (int i = 0; i < unknownX; i++)
            {
                Read_JointDefinition(reader);
            }

            if (version >= 2)
            {
                ClusterDefinition_reader0_v2B(reader);
            }
            if (version >= 4)
            {
                this.Name = ReadString(reader);
            }
            if (version >= 5)
            {
                reader.ReadUInt32();
            }
        }

        private void ClusterDefinition_reader_End_inner(BinaryReader reader)
        {
            var version = ReadVersion(reader, 1, 0x1416EE000);

            var unknownA = reader.ReadUInt64(); // objectA
            var unknownB = reader.ReadUInt64(); // objectB
            var unknownC = reader.ReadUInt32(); // jointType

            //"jointDefinition"
            ReadComponent(reader, Read_JointDefinition);
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
