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
        //private readonly Timer t = new Timer() { Enabled = true, Interval = 100 };
        //private readonly Timer t1 = new Timer() { Enabled = true, Interval = 1 };
       // private readonly Ping ping = new Ping();
       // private readonly ConcurrentBag<HttpRequestMessage> _bag = new ConcurrentBag<HttpRequestMessage>();
        private readonly ConcurrentDictionary<HttpRequestMessage, Stopwatch> _dict = new ConcurrentDictionary<HttpRequestMessage, Stopwatch>();
        //private void T_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        //{
        //    Console.WriteLine(_bag.Count);
        //}

        public ThrottleHandler()
        {
            //t.Elapsed += (s, e) =>
            //{
            //    //ThreadPool.GetAvailableThreads(out var w, out var c);
            //    Console.WriteLine($"{Process.GetCurrentProcess().Threads.Count}");
            //};
            //t1.Elapsed += (s, e) =>
            //{
            //    Console.WriteLine(_bag.Count);
            //};
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _dict.TryAdd(request, Stopwatch.StartNew());
            //Interlocked.Increment(ref _count);
            //_bag.Add(request);
            var msg = await base.SendAsync(request, cancellationToken);
            
            //if(!_bag.TryTake(out request))
            //    Console.Write("failed");
            _dict.TryGetValue(request, out var s);
            s.Stop();
            Console.WriteLine(s.ElapsedMilliseconds);
            return msg;
        }
    }
}
