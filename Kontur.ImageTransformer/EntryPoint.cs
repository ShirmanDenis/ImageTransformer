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
using System.Web.Http.Dispatcher;
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
        }
    }
}
