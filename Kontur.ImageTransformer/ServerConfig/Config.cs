using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.SelfHost;
using System.Web.Http.Validation;
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
        public static IKernel CreateKernel(HttpSelfHostConfiguration config)
        {
            config.TransferMode = TransferMode.Streamed;
            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());
            config.MessageHandlers.Add(new RouteValidator());
            config.MaxReceivedMessageSize = Int64.MaxValue;

            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());

            kernel.Bind<IImageProcessService>().To<ImageProcessService>();
            kernel.Bind<IFiltersFactory>().To<FiltersFactory.FiltersFactory>()
                .OnActivation(factory =>
                {
                    factory.RegisterFilter("threshold", new ThresholdFilter());
                    factory.RegisterFilter("sepia", new SepiaFilter());
                    factory.RegisterFilter("grayscale", new GrayscaleFilter());
                });
            kernel.Bind<IImageServiceOptions>().To<ImageServiceOptions>();

            kernel.Bind<DefaultModelValidatorProviders>()
                .ToConstant(new DefaultModelValidatorProviders(config.Services.GetServices(typeof(ModelValidatorProvider))
                    .Cast<ModelValidatorProvider>()));
            kernel.
                Bind<DefaultFilterProviders>()
                .ToConstant(new DefaultFilterProviders(new[] { new NinjectFilterProvider(kernel) }.AsEnumerable()));

            return kernel;
        }
    }
}
