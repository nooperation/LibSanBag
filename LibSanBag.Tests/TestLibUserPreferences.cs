using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibSanBag.ResourceUtils;
using NUnit.Framework;

namespace LibSanBag.Tests
{
    [TestFixture]
    class TestLibUserPreferences
    {
        public string DefaultGuid => "01234567-89ab-cdef-0123-456789abcdef";
        public string DefaultSalt => "6E3F032949637D2E";

        [Test]
        public void TestWrite()
        {
            var Input = "Hello World\0";
            var ExpectedOutput = new byte[] { 0x6a, 0x0f, 0xb1, 0xca, 0x3b, 0x60, 0x63, 0x04, 0x2c, 0x16, 0x92, 0xa2, 0x6e, 0x40, 0x71, 0x02 };

            using (var ms = new MemoryStream())
            {
                LibUserPreferences.Write(ms, Input, DefaultGuid, DefaultSalt);
                var output = ms.ToArray();

                Assert.AreEqual(ExpectedOutput, output);
            }
        }

        [Test]
        public void TestRead()
        {
            var Input = new byte[] { 0x6a, 0x0f, 0xb1, 0xca, 0x3b, 0x60, 0x63, 0x04, 0x2c, 0x16, 0x92, 0xa2, 0x6e, 0x40, 0x71, 0x02 };
            var ExpectedOutput = "Hello World\0";

            using (var ms = new MemoryStream(Input))
            {
                var output = LibUserPreferences.Read(ms, DefaultGuid, DefaultSalt);
                Assert.AreEqual(ExpectedOutput, output);
            }
        }
    }
}
