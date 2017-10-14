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

            using (var decompressedStream = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                var stringBytes = decompressedStream.ReadChars(0x66);
                ScriptSourceTextPath = new string(stringBytes);
                var assemblyLength = decompressedStream.ReadInt32();
                AssemblyBytes = decompressedStream.ReadBytes(assemblyLength);
            }
         }
    }
}
