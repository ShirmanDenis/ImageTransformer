using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.SelfHost;
using Kontur.ImageTransformer.Controller;
using Kontur.ImageTransformer.Filters;
using Kontur.ImageTransformer.ImageService;
using Kontur.ImageTransformer.ServerConfig;
using uhttpsharp;
using uhttpsharp.Attributes;
using uhttpsharp.Controllers;
using uhttpsharp.Handlers;
using uhttpsharp.Listeners;
using uhttpsharp.ModelBinders;
using uhttpsharp.RequestProviders;

namespace Kontur.ImageTransformer
{
    public class EntryPoint
    {
        public static void Main(string[] args)
        {
            using (var server = new HttpSelfHostServer(Config.Create("http://localhost:8080/")))
            {
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

            //var ms = new MemoryStream();
            //var fs = File.OpenRead("D:\\AlphaImg.png");
            //fs.CopyTo(ms);
            //var arr = ms.ToArray();
            //var tasks = new List<Task>();
            //for (int i = 0; i < 1000; i++)
            //{
            //    tasks.Add(Task.Factory.StartNew(() =>
            //    {
            //        var factory = new FiltersFactory.FiltersFactory();
            //        factory.RegisterFilter("sepia", new SepiaFilter());
            //        var controller = new ProcessController(new ImageProcessService(new ImageServiceOptions()), factory);
            //        var request =
            //            new HttpRequestMessage(HttpMethod.Post, "proccess/sepia/1,1,100,100")
            //            {
            //                Content = new ByteArrayContent(arr)
            //            };
            //        controller.Request = request;
            //        var r = controller.PostSepia(0, 0, 100, 100).Result;
            //        Console.WriteLine("Lol");
            //    }));
            //}

            //var w = Stopwatch.StartNew();
            //Task.WhenAll(tasks.ToArray()).Wait();
            //w.Stop();
            //Console.WriteLine("All: " + w.Elapsed);
            //Console.Read();
        }
    }
}
