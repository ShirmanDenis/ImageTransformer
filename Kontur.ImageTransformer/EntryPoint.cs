using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.SelfHost;
using Kontur.ImageTransformer.Filters;
using Kontur.ImageTransformer.ServerConfig;

namespace Kontur.ImageTransformer
{
    public class EntryPoint
    {
        public static void Main(string[] args)
        {
            using (var server = new HttpSelfHostServer(Config.Create("http://localhost:8080/")))
            {
                server.OpenAsync();
                Console.WriteLine("Press Enter to quit.");
                Console.Read();
            }
        }
    }
}
