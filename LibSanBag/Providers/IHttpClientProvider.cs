using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSanBag.Providers
{
    public enum ProgressStatus
    {
        Idling,
        Connecting,
        Downloading,
        Commpleted,
        Error
    }
    public struct ProgressEventArgs
    {
        public string Resource { get; set; }
        public ProgressStatus Status { get; set; }
        public long BytesDownloaded { get; set; }
        public long TotalBytes { get; set; }
        public int CurrentResourceIndex { get; set; }
        public int TotalResources { get; set; }
    }

    public interface IHttpClientProvider
    {
        Task<byte[]> GetByteArrayAsync(string requestUri, IProgress<ProgressEventArgs> progress);
    }
}
