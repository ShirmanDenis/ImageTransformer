using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.SelfHost;
using Kontur.ImageTransformer.ServerConfig;

namespace Kontur.ImageTransformer
{
    public class EntryPoint
    {
        public static void Main(string[] args)
        {
            using (var server = new HttpSelfHostServer(Config.Create("http://localhost:8080/")))
            {
                //ThreadPool.SetMinThreads(10000, 10000);
                //ThreadPool.SetMaxThreads(10000, 10000);

                try
                {
                    server.OpenAsync();
                    Console.WriteLine("Press Enter to quit.");
                    Console.ReadLine();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            }
            //var cancel = new CancellationTokenSource(500);
            //cancel.Token.Register(() =>
            //{
            //    Console.WriteLine("Canceled!");
            //});
            //cancel.CancelAfter(500);
            //Task.Factory.StartNew(() =>
            //{
            //    Thread.Sleep(1000);
            //    Console.WriteLine("Done!");
            //}, cancel.Token);
            //Console.Read();
        }
    }
}
