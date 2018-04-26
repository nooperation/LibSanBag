using System.Runtime.InteropServices;
using System.Text;

namespace LibSanBag.ResourceUtils
{
    public class LibFSB
    {
        [DllImport("LibFSB.dll")]
        private static extern bool SaveFsbAsWav(byte[] input, uint inputLength, byte[] outputPath);

        /// <summary>
        /// Saves the FSB bytes as a wav format to the specified path
        /// </summary>
        /// <param name="fsbBytes">FSB bytes</param>
        /// <param name="outputPath">Path to output WAV</param>
        /// <returns>True on success</returns>
        public static bool SaveAs(byte[] fsbBytes, string outputPath)
        {
            return SaveFsbAsWav(fsbBytes, (uint)fsbBytes.Length, Encoding.ASCII.GetBytes(outputPath));
        }
    }
}
