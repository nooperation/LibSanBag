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
    public class ScriptSourceTextResource
    {
        public string Filename { get; set; }
        public string Source { get; set; }

        public ScriptSourceTextResource(Stream sourceStream, FileRecord fileRecord)
        {
            byte[] decompressedBytes = null;
            using (var compressedStream = new MemoryStream())
            {
                fileRecord.Save(sourceStream, compressedStream);
                decompressedBytes = OodleLz.DecompressResource(compressedStream);
            }

            using (var decompressedStream = new BinaryReader(new MemoryStream(decompressedBytes)))
            {
                // TODO: Find the actual length...
                var filenameString = "";
                while (decompressedStream.BaseStream.Position < decompressedStream.BaseStream.Length)
                {
                    filenameString += decompressedStream.ReadChar();
                    if (filenameString.EndsWith(".cs"))
                    {
                        break;
                    }
                }

                Filename = filenameString;
                var assemblyLength = decompressedStream.ReadInt32();
                Source = new string(decompressedStream.ReadChars(assemblyLength));
            }
        }
    }
}
