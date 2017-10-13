using SanBag.ResourceUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanBag.Models
{
    class ScriptSourceTextResource
    {
        public class ScriptSourceText
        {
            public string Filename { get; set; }
            public string Source { get; set; }
        }

        public static ScriptSourceText ExtractScriptSourceText(byte[] compressedBytes)
        {
            var scriptSourceText = new ScriptSourceText();
            byte[] decompressedSourceBytes = null;

            using (var compressedStream = new MemoryStream(compressedBytes))
            {
                decompressedSourceBytes = OodleLz.DecompressResource(compressedStream);
            }

            using (var decompressedStream = new MemoryStream(decompressedSourceBytes))
            {
                using (var br = new BinaryReader(decompressedStream))
                {
                    // TODO: Find the actual length...
                    var filenameString = "";
                    while (br.BaseStream.Position < br.BaseStream.Length)
                    {
                        filenameString += br.ReadChar();
                        if (filenameString.EndsWith(".cs"))
                        {
                            break;
                        }
                    }

                    scriptSourceText.Filename = filenameString;
                    var assemblyLength = br.ReadInt32();
                    scriptSourceText.Source = new string(br.ReadChars(assemblyLength));
                }
            }

            return scriptSourceText;
        }
    }
}
