using Kontur.ImageTransformer.ServerConfig;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Kontur.ImageTransformer
{
    public class EntryPoint
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Config>();
    }
}
