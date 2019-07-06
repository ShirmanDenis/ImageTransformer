using System.Reflection;
using Kontur.ImageTransformer.ImageFilters;
using Kontur.ImageTransformer.FiltersFactory;
using Kontur.ImageTransformer.ImageService;
using Kontur.ImageTransformer.ModelBinders;
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
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddMvcOptions(options =>
                {
                    options.ModelBinderProviders.Insert(0, new FilterModelBinderProvider());
                });
            services.AddSingleton<ImageServiceOptions>();
            services.AddSingleton<IImageProcessService, ImageProcessService>();

            services.AddSingleton<IFiltersFactory>(_ =>
            {
                var factory = new FiltersFactory.FiltersFactory();
                factory.RegisterFilter("threshold", new ThresholdFilter());
                factory.RegisterFilter("sepia", new SepiaFilter());
                factory.RegisterFilter("grayscale", new GrayscaleFilter());
                return factory;
            });

            var sp = services.BuildServiceProvider();
            var resolver = new FilterByRouteResolver(sp.GetService<IFiltersFactory>());
            resolver.AddRouteValidator(@"(?<FilterName>threshold)\((?<params>[0-9]{1,3})\)");
            resolver.AddRouteValidator(@"(?<FilterName>sepia)");
            resolver.AddRouteValidator(@"(?<FilterName>grayscale)");

            services.AddSingleton<IFilterByRouteResolver>(resolver);
            services.AddSingleton<IParamsFromRouteExtractor>(resolver);


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
