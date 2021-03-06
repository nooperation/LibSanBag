﻿using LibSanBag.FileResources;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

namespace LibSanBag.Tests.FileResources
{
    [TestFixture]
    internal class TestLuaScriptResource : BaseFileResourceTest
    {
        // TODO: Create some actual lua samples to mimic the compressed havok lua scripts
        private struct TestData
        {
            public string CompressedFilePath { get; }
            public string ExpectedFileName { get; }
            public FileRecordInfo RecordInfo { get; }

            public TestData(string compressedFilePath, string expectedFileName)
            {
                CompressedFilePath = Path.Combine(RootPath, compressedFilePath);
                RecordInfo = FileRecordInfo.Create(compressedFilePath);
                ExpectedFileName = expectedFileName;
            }
        }

        private static readonly string RootPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Samples", "Resources", "ScriptSourceText");
        private static readonly string ExpectedFilePath = Path.Combine(RootPath, "ScriptSourceText-Resource.cs");

        private string ExpectedSource { get; set; }
        private IEnumerable<TestData> Tests { get; } = new[]
        {
            new TestData("63d3d75933432b36adca64c6d778a1d7.ScriptSourceText-Resource.v6301a7d31aa6f628.payload.v0.noVariants", "ExampleScript.cs"),
        };

        [SetUp]
        public override void Setup()
        {
            base.Setup();

            ExpectedSource = File.ReadAllText(ExpectedFilePath);
        }

        [Test]
        public void TestConstructCompressedStream()
        {
            foreach (var testData in Tests)
            {
                var compressedFileBytes = File.ReadAllBytes(testData.CompressedFilePath);

                using (var ms = new MemoryStream(compressedFileBytes))
                {
                    var resource = LuaScriptResource.Create(testData.RecordInfo.VersionHash);
                    resource.InitFromStream(ms);
                    Assert.AreEqual(resource.Resource.Filename, testData.ExpectedFileName);
                    Assert.AreEqual(resource.Resource.Source, ExpectedSource);
                }
            }
        }

        [Test]
        public void TestConstructFileInfo()
        {
            foreach (var testData in Tests)
            {
                var fileStream = File.OpenRead(testData.CompressedFilePath);
                var fileRecord = new FileRecord
                {
                    Length = (uint)fileStream.Length,
                    Info = null,
                    Offset = 0,
                    TimestampNs = 0,
                    Name = "File Record"
                };

                var resource = LuaScriptResource.Create(testData.RecordInfo.VersionHash);
                resource.InitFromRecord(fileStream, fileRecord);
                Assert.AreEqual(resource.Resource.Filename, testData.ExpectedFileName);
                Assert.AreEqual(resource.Resource.Source, ExpectedSource);
            }
        }

        [Test]
        public void TestConstructBytes()
        {
            foreach (var testData in Tests)
            {
                var filebytes = File.ReadAllBytes(testData.CompressedFilePath);

                var resource = LuaScriptResource.Create(testData.RecordInfo.VersionHash);
                resource.InitFromRawCompressed(filebytes);
                Assert.AreEqual(resource.Resource.Filename, testData.ExpectedFileName);
                Assert.AreEqual(resource.Resource.Source, ExpectedSource);
            }
        }
    }
}
