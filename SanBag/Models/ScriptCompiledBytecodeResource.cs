using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanBag.Models
{
    class ScriptCompiledBytecodeResource
    {
        public class ScriptCompiledBytecode
        {
            public string ScriptSourceTextPath { get; set; }
            public byte[] AssemblyBytes { get; set; }
        }

        public static ScriptCompiledBytecode ExtractAssembly(byte[] fileBytes)
        {
            var assemblyData = new ScriptCompiledBytecode();

            using (var ms = new MemoryStream(fileBytes))
            {
                using (var br = new BinaryReader(ms))
                {
                    var stringBytes = br.ReadChars(0x66);
                    assemblyData.ScriptSourceTextPath = new string(stringBytes);
                    var assemblyLength = br.ReadInt32();
                    assemblyData.AssemblyBytes = br.ReadBytes(assemblyLength);
                }
            }

            return assemblyData;
        }
    }
}
