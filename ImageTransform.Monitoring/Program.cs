using System;
using System.Drawing;
using ImageTransform.Client.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ImageTransform.Monitoring
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var model = new FiltrateImageModel()
            {
                Area = new Rectangle(1, 2, 100, 600)
            };
            var json = JsonConvert.SerializeObject(model, Formatting.Indented);

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://localhost:3500")
                .UseStartup<Startup>();
    }
}
