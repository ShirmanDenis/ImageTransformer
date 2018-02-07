using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Timer = System.Timers.Timer;

namespace Kontur.ImageTransformer.ServerConfig
{
    public class ThrottleHandler : DelegatingHandler
    {
        private int _setted = 800;
        private readonly ConcurrentDictionary<HttpRequestMessage, DateTime> _dict = new ConcurrentDictionary<HttpRequestMessage, DateTime>();
        private int rps = 0;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            rps = Calc();

            if (rps >= _setted)
                return request.CreateResponse(429);
            _dict.TryAdd(request, DateTime.Now);

            var msg = await base.SendAsync(request, cancellationToken);

            var cancelSource = new CancellationTokenSource(1000);
            cancelSource.Token.Register(() => _dict.TryRemove(request, out var t));
            
            return msg;
        }

        private int Calc()
        {
            return rps = _dict.Values.Count(t => Comp(t, DateTime.Now));
        }

        private bool Comp(DateTime l, DateTime r)
        {
            return l.Day == r.Day && l.Hour == r.Hour && l.Minute == r.Minute && l.Second == r.Second;
        }
    }
}
