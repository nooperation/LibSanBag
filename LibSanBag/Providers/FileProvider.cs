using System.IO;

namespace LibSanBag.Providers
{
    public class FileProvider : IFileProvider
    {
        public bool FileExists(string path)
        {
            return File.Exists(path);
        }
    }
}
