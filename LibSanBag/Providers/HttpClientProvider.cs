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
        public Task<byte[]> GetByteArrayAsync(string requestUri)
        {
            using (var client = new HttpClient())
            {
                return client.GetByteArrayAsync(requestUri);
            }
        }
    }
}
