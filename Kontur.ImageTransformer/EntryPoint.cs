using System;
using System.Web.Http.SelfHost;
using Kontur.ImageTransformer.ServerConfig;

namespace Kontur.ImageTransformer
{
    public class EntryPoint
    {
        public static void Main(string[] args)
        {
            var configuration = Config.Create("http://localhost:8080/");
            using (var server = new HttpSelfHostServer(configuration))
            {
                server.OpenAsync();
                Console.WriteLine("Press Enter to quit.");
                Console.Read();
            }
        }
    }
}
