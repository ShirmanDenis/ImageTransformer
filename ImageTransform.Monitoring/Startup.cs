using ImageTransform.Client;
using ImageTransform.Monitoring.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Vostok.Logging.Abstractions;
using Vostok.Logging.File;

namespace ImageTransform.Monitoring
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<MonitoringSettings>(Configuration.GetSection("MonitoringSettings"));
            services.AddMvc(options => options.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddSingleton<ILog>(sp => new FileLog(sp.GetService<IOptions<MonitoringSettings>>().Value.FileLogSettings));
            services.AddSingleton(sp => ImageTransformClientFactory.Create(sp.GetService<IOptions<MonitoringSettings>>().Value.ApiUrl, sp.GetService<ILog>()));

            services.AddSpaStaticFiles(options => options.RootPath = "ReactApp\\build");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseSpaStaticFiles();
            app.UseSpa(builder =>
            {
                builder.Options.SourcePath = "ReactApp";
                builder.Options.DefaultPageStaticFileOptions = new StaticFileOptions()
                {
                    OnPrepareResponse = DisableCacheIndexPage
                };

                void DisableCacheIndexPage(StaticFileResponseContext responseContext)
                {
                    if (responseContext.File.Name != "index.html")
                        return;

                    var headers = responseContext.Context.Response.GetTypedHeaders();
                    var cacheControlHeaderValue = new CacheControlHeaderValue { NoStore = true, NoCache = true };

                    headers.CacheControl = cacheControlHeaderValue;
                }

            });

        }
    }
}
