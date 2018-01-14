using System;
using System.Drawing;
using System.Threading;
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
                server.OpenAsync();

                Console.WriteLine("Press Enter to quit.");
                Console.ReadLine();
            }
        }
    }
}
