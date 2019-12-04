﻿using Newtonsoft.Json;
using System.IO;

namespace LibSanBag.FileResources
{
    public class SoundResource : BaseFileResource
    {
        public override bool IsCompressed => true;
        public uint Version { get; set; }
        public SoundResourceData Sound { get; set; }

        public static SoundResource Create(string version = "")
        {
            return new SoundResource();
        }

        public class SoundResourceData
        {
            public uint Version { get; set; }
            public string Name { get; set; }

            [JsonIgnore]
            public byte[] Data { get; set; }
            public int DataLength => Data?.Length ?? 0;
            public uint DataType { get; set; }
            public float LengthSeconds { get; set; }
            public float PeakLoudness { get; set; }
        }
        private SoundResourceData Read_SoundResource(BinaryReader reader)
        {
            var result = new SoundResourceData();

            result.Version = ReadVersion(reader, 3, 0x1411ABAC0);
            result.Name = ReadString(reader);
            result.Data = Read_Array(reader);

            if (result.Version >= 2)
            {
                result.DataType = reader.ReadUInt32();
            }
            if (result.Version >= 3)
            {
                result.LengthSeconds = reader.ReadSingle();
                result.PeakLoudness = reader.ReadSingle();
            }

            return result;
        }

        public override void InitFromRawDecompressed(byte[] decompressedBytes)
        {
            using (var reader = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                this.Version = ReadVersion(reader, 1, 0x1410E3B40);
                this.Sound = ReadComponent(reader, Read_SoundResource);
            }
        }
    }
}
