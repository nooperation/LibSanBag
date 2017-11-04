using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibSanBag.Providers;

namespace LibSanBag.Tests.Providers
{
    public class MockFileProvider : IFileProvider
    {
        public Queue<bool> FileExistsResultQueue { get; } = new Queue<bool>();

        public bool FileExists(string path)
        {
            return FileExistsResultQueue.Dequeue();
        }
    }
}
