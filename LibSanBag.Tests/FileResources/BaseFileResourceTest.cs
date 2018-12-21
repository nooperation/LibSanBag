using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibSanBag.Providers;
using LibSanBag.ResourceUtils;
using NUnit.Framework;

namespace LibSanBag.Tests.FileResources
{
    class BaseFileResourceTest
    {
        [SetUp]
        public virtual void Setup()
        {
            if (Unpacker.IsAvailable == false)
            {
                Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
                Console.WriteLine("BaseFileResourceTest: Setting current directory to " +  Environment.CurrentDirectory);
                Unpacker.FindDependencies(new FileProvider());
            }
        }
    }
}
