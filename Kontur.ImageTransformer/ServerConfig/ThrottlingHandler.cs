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
        private readonly int threshold;
        private readonly ConcurrentDictionary<HttpRequestMessage, Stopwatch> _collectedRequests = new ConcurrentDictionary<HttpRequestMessage, Stopwatch>();

        public ThrottlingHandler(int avgScriptExecTimeInMiliSeconds)
        {
            threshold = avgScriptExecTimeInMiliSeconds;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var avg = 0.0;
            var times = _collectedRequests.Values.ToArray();
            if (times.Length > 0)
                avg = times.Average(t => t.ElapsedMilliseconds);
            if (avg > threshold)
            {
                return request.CreateResponse(429);
            }

            _collectedRequests.TryAdd(request, Stopwatch.StartNew());
            var msg =  await base.SendAsync(request, cancellationToken);
            _collectedRequests.TryRemove(request, out var s);

            return msg;
        }
    }
}
