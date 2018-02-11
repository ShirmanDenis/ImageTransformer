using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Kontur.ImageTransformer.ServerConfig
{
    public class ThrottlingHandler : DelegatingHandler
    {
        private double _avg;
        private readonly ConcurrentDictionary<HttpRequestMessage, Stopwatch> _collectedRequests = new ConcurrentDictionary<HttpRequestMessage, Stopwatch>();

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!_collectedRequests.IsEmpty)
                _avg =  Interlocked.Exchange(ref _avg, _collectedRequests.Values.Average(t => t.ElapsedMilliseconds));
            if (_avg > 500)
                return request.CreateResponse(429);
            try
            {
                _collectedRequests.TryAdd(request, Stopwatch.StartNew());
                return await base.SendAsync(request, cancellationToken);
            }
            finally 
            {
                _collectedRequests.TryRemove(request, out var _);
            }
        }
    }
}
