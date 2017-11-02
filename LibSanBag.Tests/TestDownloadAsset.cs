using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibSanBag.Providers;
using NUnit.Framework;

namespace LibSanBag.Tests
{
    [TestFixture(Category = nameof(TestDownloadAsset))]
    class TestDownloadAsset
    {
        public string ResourceId { get; } = "00000000";
        public FileRecordInfo.ResourceType ResourceType { get; } = FileRecordInfo.ResourceType.TextureResource;
        public FileRecordInfo.PayloadType PayloadType { get; } = FileRecordInfo.PayloadType.Payload;
        public FileRecordInfo.VariantType VariantType { get; } = FileRecordInfo.VariantType.NoVariants;
        public byte[] ExpectedResult { get; } = Encoding.ASCII.GetBytes("OK");

        public string AssetVersion => AssetVersions.GetResourceVersions(ResourceType)[0];

        public string AssetName
        {
            get
            {
                var resourceTypeName = FileRecordInfo.GetResourceTypeName(ResourceType);
                var payloadTypeName = FileRecordInfo.GetPayloadTypeName(PayloadType);
                var variantTypeName = FileRecordInfo.GetVariantTypeName(VariantType);

                return $"{ResourceId}.{resourceTypeName}.v{AssetVersion}.{payloadTypeName}.v0.{variantTypeName}";
            }
        }

        public class MockHttpClient : IHttpClientProvider
        {
            public class ClientAction
            {
                public string ExpectedAddress { get; set; }
                public byte[] ReturnedValue { get; set; }
                public Exception ThrownException { get; set; }
            }

            public Queue<ClientAction> ClientActionQueue { get; set; } = new Queue<ClientAction>();

            public Task<byte[]> GetByteArrayAsync(string requestUri)
            {
                var currentClientAction = ClientActionQueue.Dequeue();
                if (currentClientAction.ThrownException != null)
                {
                    throw currentClientAction.ThrownException;
                }

                if (currentClientAction.ExpectedAddress != null)
                {
                    Assert.AreEqual(currentClientAction.ExpectedAddress, requestUri);
                }

                var task = new Task<byte[]>(() => currentClientAction.ReturnedValue);
                task.Start();
                return task;
            }
        }

        [Test]
        public void TestDownloadResourceAsyncAllVersions()
        {
            var mockClient = new MockHttpClient();
            mockClient.ClientActionQueue.Enqueue(new MockHttpClient.ClientAction()
            {
                ExpectedAddress = $"http://sansar-asset-production.s3-us-west-2.amazonaws.com/{AssetName}",
                ReturnedValue = ExpectedResult,
                ThrownException = null
            });

            var result = FileRecordInfo.DownloadResourceAsync(ResourceId, ResourceType, PayloadType, VariantType, mockClient).Result;
            Assert.AreEqual(ExpectedResult, result.Bytes);
            Assert.AreEqual(AssetName, result.Name);
            Assert.AreEqual(AssetVersion, result.Version);
        }

        [Test]
        public void TestDownloadResourceAsyncException()
        {
            var mockClient = new MockHttpClient();
            mockClient.ClientActionQueue.Enqueue(new MockHttpClient.ClientAction()
            {
                ExpectedAddress = $"http://sansar-asset-production.s3-us-west-2.amazonaws.com/{AssetName}",
                ReturnedValue = ExpectedResult,
                ThrownException = new Exception()
            });

            Assert.Throws<AggregateException>(() =>
            {
                var result = FileRecordInfo.DownloadResourceAsync(ResourceId, ResourceType, PayloadType, VariantType, mockClient).Result;
            });
        }

        [Test]
        public void TestDownloadResourceAsynsUnknown()
        {
            var mockClient = new MockHttpClient();
            mockClient.ClientActionQueue.Enqueue(new MockHttpClient.ClientAction()
            {
                ExpectedAddress = null,
                ReturnedValue = ExpectedResult,
                ThrownException = null
            });

            var result = FileRecordInfo.DownloadResourceAsync(ResourceId, FileRecordInfo.ResourceType.Unknown, PayloadType, VariantType, mockClient).Result;
            Assert.AreEqual(null, result);
        }

        [Test]
        public void TestDownloadResourceAsyncSpecificVersion()
        {
            var mockClient = new MockHttpClient();
            mockClient.ClientActionQueue.Enqueue(new MockHttpClient.ClientAction()
            {
                ExpectedAddress = $"http://sansar-asset-production.s3-us-west-2.amazonaws.com/{AssetName}",
                ReturnedValue = ExpectedResult,
                ThrownException = null
            });


            var result = FileRecordInfo.DownloadResourceAsync(ResourceId, ResourceType, PayloadType, VariantType, AssetVersion, mockClient).Result;
            Assert.AreEqual(ExpectedResult, result.Bytes);
            Assert.AreEqual(AssetName, result.Name);
            Assert.AreEqual(AssetVersion, result.Version);
        }
    }
}
