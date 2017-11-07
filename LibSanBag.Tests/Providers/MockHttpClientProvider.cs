using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibSanBag.Providers;
using NUnit.Framework;

namespace LibSanBag.Tests.Providers
{
    public class MockHttpClientProvider : IHttpClientProvider
    {
        public class ClientAction
        {
            public string ExpectedAddress { get; set; }
            public byte[] ReturnedValue { get; set; }
            public Exception ThrownException { get; set; }
        }

        public Queue<ClientAction> ClientActionQueue { get; } = new Queue<ClientAction>();

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
}
