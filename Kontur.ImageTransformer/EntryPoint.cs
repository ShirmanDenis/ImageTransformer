using System;
using System.Web.Http.SelfHost;
using Kontur.ImageTransformer.ServerConfig;

namespace Kontur.ImageTransformer
{
    public class EntryPoint
    {
        public static void Main(string[] args)
        {          
            using (var server = new HttpSelfHostServer(Config.CreateConfig("http://localhost:8080/")))
            {
                server.OpenAsync().Wait();

                Console.WriteLine("Press Enter to quit.");
                Console.ReadLine();
            }
        }
    }
}
