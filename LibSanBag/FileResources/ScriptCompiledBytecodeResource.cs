using LibSanBag;
using LibSanBag.ResourceUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSanBag.FileResources
{
    public class ScriptCompiledBytecodeResource
    {
        public string ScriptSourceTextPath { get; set; }
        public byte[] AssemblyBytes { get; set; }

        public ScriptCompiledBytecodeResource(Stream sourceStream, FileRecord fileRecord)
        {
            byte[] decompressedBytes = null;
            using (var compressedStream = new MemoryStream())
            {
                fileRecord.Save(sourceStream, compressedStream);
                decompressedBytes = OodleLz.DecompressResource(compressedStream);
            }

            InitFrom(decompressedBytes);
        }

        public ScriptCompiledBytecodeResource(Stream compressedStream)
        {
            var decompressedBytes = OodleLz.DecompressResource(compressedStream);

            InitFrom(decompressedBytes);
        }

        public ScriptCompiledBytecodeResource(byte[] compressedBytes)
        {
            byte[] decompressedBytes = null;

            using (var compressedStream = new MemoryStream(compressedBytes))
            {
                decompressedBytes = OodleLz.DecompressResource(compressedStream);
            }

            InitFrom(decompressedBytes);
        }

        private void InitFrom(byte[] decompressedStream)
        {
            using (var br = new BinaryReader(new MemoryStream(decompressedStream)))
            {
                var stringLength = br.ReadInt32();
                var stringChars = br.ReadChars(stringLength);
                ScriptSourceTextPath = new string(stringChars);

                var assemblyLength = br.ReadInt32();
                AssemblyBytes = br.ReadBytes(assemblyLength);
            }
        }
    }
}
