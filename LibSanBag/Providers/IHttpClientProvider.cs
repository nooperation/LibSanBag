using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSanBag.Providers
{
    public struct ProgressEventArgs
    {
        public string Resource { get; set; }
        public long Downloaded { get; set; }
        public long Total { get; set; }
    }

    public interface IHttpClientProvider
    {
        Task<byte[]> GetByteArrayAsync(string requestUri, IProgress<ProgressEventArgs> progress);
    }
}
