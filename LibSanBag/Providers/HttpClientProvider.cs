using System;
using System.Net;
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
                            Status = ProgressStatus.Downloading,
                            Resource = requestUri,
                            BytesDownloaded = args.BytesReceived,
                            TotalBytes = args.TotalBytesToReceive,
                            CurrentResourceIndex = 0,
                            TotalResources = 1
                        }
                    );
                };

                progress.Report(
                    new ProgressEventArgs()
                    {
                        Status = ProgressStatus.Connecting,
                        Resource = requestUri,
                        BytesDownloaded = 0,
                        TotalBytes = 1,
                        TotalResources = 1,
                        CurrentResourceIndex = 0
                    }
                );
            }

            return _client.DownloadDataTaskAsync(requestUri);
        }
    }
}
