using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LibSanBag.Providers
{
    public class HttpClientProvider : IHttpClientProvider
    {
        private static readonly HttpClient Client = new HttpClient();

        public Task<byte[]> GetByteArrayAsync(string requestUri)
        {
            return Client.GetByteArrayAsync(requestUri);
        }
    }
}
