using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSanBag.Providers
{
    public struct ProgressEventArgs
    {
        public long Downloaded { get; set; }
        public long Total { get; set; }
    }

    public interface IHttpClientProvider
    {
        event EventHandler<ProgressEventArgs> OnProgress;

        Task<byte[]> GetByteArrayAsync(string requestUri);
    }
}
