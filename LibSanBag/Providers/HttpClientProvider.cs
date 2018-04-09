using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LibSanBag.Providers
{
    public class HttpClientProvider : IHttpClientProvider
    {
        private readonly WebClient _client = new WebClient();

        public Task<byte[]> GetByteArrayAsync(string requestUri, IProgress<ProgressEventArgs> progress = null)
        {
            if (progress != null)
            {
                _client.DownloadProgressChanged += (source, args) =>
                {
                    progress.Report(
                        new ProgressEventArgs()
                        {
                            Resource = requestUri,
                            Downloaded = args.BytesReceived,
                            Total = args.TotalBytesToReceive
                        }
                    );
                };
            }

            return _client.DownloadDataTaskAsync(requestUri);
        }
    }
}
