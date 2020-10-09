using LibSanBag;
using LibSanBag.ResourceUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSanBag.FileResources
{
    public class AudioMaterialResource : BaseFileResource
    {
        public static AudioMaterialResource Create(string version = "")
        {
            return new AudioMaterialResource();
        }

        public class AudioMaterial
        {
            public uint Version { get; internal set; }
            public float Absorption0 { get; internal set; }
            public float Absorption1 { get; internal set; }
            public float Absorption2 { get; internal set; }
            public List<float> Absorpotion { get; internal set; }
            public float Scattering { get; internal set; }
            public string Name { get; internal set; }
            public string FootstepGraph { get; internal set; }
            public string ImpactObjectLightGraph { get; internal set; }
            public string ImpactObjectMediumGraph { get; internal set; }
            public string ImpactObjectHeavyGraph { get; internal set; }
            public string ImpactSurfaceLightGraph { get; internal set; }
            public string ImpactSurfaceMediumGraph { get; internal set; }
            public string ImpactSurfaceHeavyGraph { get; internal set; }
            public string RollGraph { get; internal set; }
            public string SlideGraph { get; internal set; }
            public float Transmission0 { get; internal set; }
            public float Transmission1 { get; internal set; }
            public float Transmission2 { get; internal set; }
            public List<float> Transmission { get; internal set; }
            public string BankResource { get; internal set; }
            public string FootstepEvent { get; internal set; }
            public string ImpactEvent { get; internal set; }
            public string SurfaceEvent { get; internal set; }
        }

        private AudioMaterial Read_AudioMaterialResource(BinaryReader reader)
        {
            var result = new AudioMaterial();

            result.Version = ReadVersion(reader, 6, 0x14118B2F0);

            if (result.Version < 5)
            {
                result.Absorption0 = reader.ReadSingle();
                result.Absorption1 = reader.ReadSingle();
                result.Absorption2 = reader.ReadSingle();
            }
            else
            {
                result.Absorpotion = ReadVectorF(reader, 4);
            }

            result.Scattering = reader.ReadSingle();
            if(result.Version >= 2)
            {
                result.Name = ReadString(reader);
                if (result.Version < 6)
                {
                    result.FootstepGraph = ReadUUID(reader);
                }
            }

            if(result.Version >= 3 && result.Version <= 5)
            {
                result.ImpactObjectLightGraph = ReadUUID(reader);
                result.ImpactObjectMediumGraph = ReadUUID(reader);
                result.ImpactObjectHeavyGraph = ReadUUID(reader);
                result.ImpactSurfaceLightGraph = ReadUUID(reader);
                result.ImpactSurfaceMediumGraph = ReadUUID(reader);
                result.ImpactSurfaceHeavyGraph = ReadUUID(reader);
                result.RollGraph = ReadUUID(reader);
                result.SlideGraph = ReadUUID(reader);
            }
            if(result.Version >= 4)
            {
                if(result.Version < 5)
                {
                    result.Transmission0 = reader.ReadSingle();
                    result.Transmission1 = reader.ReadSingle();
                    result.Transmission2 = reader.ReadSingle();
                }
                else
                {
                    result.Transmission = ReadVectorF(reader, 4);
                }

                result.BankResource = ReadUUID(reader);
                result.FootstepEvent = ReadUUID_B(reader);
                result.ImpactEvent = ReadUUID_B(reader);
                result.SurfaceEvent = ReadUUID_B(reader);
            }

            return result;
        }

        public AudioMaterial Resource { get; set; }
        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var reader = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                this.Resource = Read_AudioMaterialResource(reader);
            }
        }
    }
}
