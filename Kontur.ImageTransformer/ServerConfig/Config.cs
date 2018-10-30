using System.Reflection;
using Kontur.ImageTransformer.Filters;
using Kontur.ImageTransformer.FiltersFactory;
using Kontur.ImageTransformer.ImageService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Kontur.ImageTransformer.ServerConfig
{
    public class Config
    {
        public Config(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSingleton<ImageServiceOptions>();
            services.AddSingleton<IImageProcessService, ImageProcessService>();
            services.AddSingleton<IFilterByRouteResolver>(sp =>
            {
                var resolver = new FilterByRouteResolver(sp.GetService<IFiltersFactory>());
                resolver.AddRouteValidator(@"(?<FilterName>threshold\([0-9]{1-3}\))");
                resolver.AddRouteValidator(@"(?<FilterName>sepia)");
                resolver.AddRouteValidator(@"(?<FilterName>grayscale)");
                return resolver;
            });
            services.AddSingleton<IFiltersFactory>(sp =>
            {
                var factory = new FiltersFactory.FiltersFactory();
                factory.RegisterFilter("threshold", new ThresholdFilter());
                factory.RegisterFilter("sepia", new SepiaFilter());
                factory.RegisterFilter("grayscale", new GrayscaleFilter());
                return factory;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            // link wwwroot folder
            app.UseStaticFiles();

            app.UseMvc();
        }

        //public static HttpSelfHostConfiguration Create(string baseAdress)
        //{
        //    config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());
        //    config.MessageHandlers.Add(new ThrottlingHandler(500));
        //    config.MessageHandlers.Add(new RouteValidator());
        //}
    }

}
