using ImageTransform.Api.FiltersFactory;
using ImageTransform.Api.ImageFilters;
using ImageTransform.Api.ImageService;
using ImageTransform.Api.Middlewares;
using ImageTransform.Api.ModelBinders;
using ImageTransform.Api.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Vostok.Logging.Abstractions;
using Vostok.Logging.File;

namespace ImageTransform.Api.ServerConfig
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
            services.Configure<ApiSettings>(Configuration.GetSection("ApiSettings"));
            services.AddSingleton<ILog>(sp => 
                new FileLog(sp.GetService<IOptions<ApiSettings>>().Value.FileLogSettings));
            services.AddSingleton<ImageServiceOptions>();
            services.AddSingleton<IImageProcessService, ImageProcessService>();

            services.AddSingleton<IFiltersFactory>(sp =>
            {
                var factory = new ImageTransform.Api.FiltersFactory.FiltersFactory();
                factory.RegisterFilter("threshold", new ThresholdFilter(sp.GetService<ILog>()));
                factory.RegisterFilter("sepia", new SepiaFilter());
                factory.RegisterFilter("grayscale", new GrayscaleFilter());
                return factory;
            });

            var serviceProvider = services.BuildServiceProvider();
            var resolver = new FilterByRouteResolver(serviceProvider.GetService<IFiltersFactory>());
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

            app.UseMiddleware<LoggingMiddleware>();
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
