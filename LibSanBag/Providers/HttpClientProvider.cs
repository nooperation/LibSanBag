﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LibSanBag.Providers
{
    public class HttpClientProvider : IHttpClientProvider
    {
        public event EventHandler<ProgressEventArgs> OnProgress;

        private readonly HttpClient _client = new HttpClient();

        private void RaiseProgress(long downloaded, long total)
        {
            OnProgress?.Invoke(this, new ProgressEventArgs()
            {
                Downloaded = downloaded,
                Total = total
            });
        }

        public async Task<byte[]> GetByteArrayAsync(string requestUri)
        {
            var response = await _client.GetAsync(requestUri);

            var readBuff = new byte[8193];
            using (var inBuffStream = new MemoryStream())
            {
                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    var contentLength = response.Content.Headers?.ContentLength ?? 0;
                    while (responseStream.CanRead)
                    {
                        var bytesRead = await responseStream.ReadAsync(readBuff, 0, readBuff.Length);
                        if (bytesRead == 0)
                        {
                            return inBuffStream.GetBuffer();
                        }

                        await inBuffStream.WriteAsync(readBuff, 0, bytesRead);
                        RaiseProgress(inBuffStream.Length, contentLength);
                    }
                }
            }

            return await _client.GetByteArrayAsync(requestUri);
        }
    }
}
