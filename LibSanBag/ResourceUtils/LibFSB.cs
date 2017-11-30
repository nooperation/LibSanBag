using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LibSanBag.ResourceUtils
{
    public class LibFSB
    {
        [DllImport("LibFSB.dll")]
        private static extern bool SaveFsbAsWav(byte[] input, uint inputLength, byte[] outputPath);

        public static bool SaveAs(byte[] fsbBytes, string outputPath)
        {
            return SaveFsbAsWav(fsbBytes, (uint)fsbBytes.Length, Encoding.ASCII.GetBytes(outputPath));
        }
    }
}
