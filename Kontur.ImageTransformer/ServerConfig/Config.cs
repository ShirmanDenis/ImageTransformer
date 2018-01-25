using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.SelfHost;
using System.Web.Http.Validation;
using Kontur.ImageTransformer.Controller;
using Kontur.ImageTransformer.Filters;
using Kontur.ImageTransformer.FiltersFactory;
using Kontur.ImageTransformer.ImageService;
using Ninject;
using Ninject.Web.WebApi;
using Ninject.Web.WebApi.Filter;

namespace Kontur.ImageTransformer.ServerConfig
{
    public static class Config
    {
        public static HttpSelfHostConfiguration Create(string baseAdress)
        {
            var config = new HttpSelfHostConfiguration(baseAdress);
            var kernel = new StandardKernel();
            config.MapHttpAttributeRoutes();
            config.TransferMode = TransferMode.Streamed;
            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());
            config.MessageHandlers.Add(new RouteValidator());
            config.MaxReceivedMessageSize = 1024 * 101;
            
            kernel.Load(Assembly.GetExecutingAssembly());
            kernel.Bind<IImageProcessService>().To<ImageProcessService>().InSingletonScope();
            kernel.Bind<IFiltersFactory>().To<FiltersFactory.FiltersFactory>()
                .InSingletonScope()
                .OnActivation(factory =>
                {
                    factory.RegisterFilter("threshold", new ThresholdFilter());
                    factory.RegisterFilter("sepia"    , new SepiaFilter());
                    factory.RegisterFilter("grayscale", new GrayscaleFilter());
                });

            kernel.Bind<IImageServiceOptions>().To<ImageServiceOptions>().InSingletonScope();

            kernel
                .Bind<DefaultModelValidatorProviders>()
                .ToConstant(new DefaultModelValidatorProviders(config.Services.GetServices(typeof(ModelValidatorProvider))
                .Cast<ModelValidatorProvider>()));
            kernel
                .Bind<DefaultFilterProviders>()
                .ToConstant(new DefaultFilterProviders(new[] { new NinjectFilterProvider(kernel) }.AsEnumerable()));

            config.DependencyResolver = new NinjectDependencyResolver(kernel);

            return config;
        }
    }
}
