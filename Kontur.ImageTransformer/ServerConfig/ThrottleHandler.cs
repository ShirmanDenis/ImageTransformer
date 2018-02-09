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
using System.Web.Http;
using System.Web.Http.Dispatcher;
using ThreadState = System.Diagnostics.ThreadState;
using Timer = System.Timers.Timer;

namespace Kontur.ImageTransformer.ServerConfig
{
    public class ThrottleHandler : DelegatingHandler
    {
        //private readonly ConcurrentDictionary<HttpRequestMessage, DateTime> _dict = new ConcurrentDictionary<HttpRequestMessage, DateTime>();
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(500);
        private readonly Timer _t = new Timer(){Enabled = true, Interval = 1};

        public ThrottleHandler()
        {
            _t.Elapsed += (sender, args) =>
            {
                Console.WriteLine(_semaphore.CurrentCount);
            };
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //Console.WriteLine(_semaphore.CurrentCount);
            //if (_semaphore.CurrentCount == 0)
            //{
            //    Console.WriteLine(".!.");
            //    return request.CreateResponse(429);
            //}
            await _semaphore.WaitAsync();
            var msg = await base.SendAsync(request, cancellationToken);
            _semaphore.Release();
            return msg;
        }
    }
}
