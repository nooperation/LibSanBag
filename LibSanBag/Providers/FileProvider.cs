using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSanBag.Providers
{
    class FileProvider : IFileProvider
    {
        public bool FileExists(string path)
        {
            return File.Exists(path);
        }
    }
}
