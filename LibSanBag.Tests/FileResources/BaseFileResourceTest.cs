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
            if (LibDDS.IsAvailable == false)
            {
                Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
                LibDDS.FindDependencies(new FileProvider());
            }
            if (OodleLz.IsAvailable == false)
            {
                Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
                OodleLz.FindDependencies(new FileProvider());
            }
        }
    }
}
