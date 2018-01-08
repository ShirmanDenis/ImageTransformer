using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Batch;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;
using System.Web.Http.SelfHost;
using System.Web.Http.Validation;
using Kontur.ImageTransformer.Filters;
using Kontur.ImageTransformer.FiltersFactory;
using Kontur.ImageTransformer.ImageService;
using Kontur.ImageTransformer.ServerConfig;
using Ninject;
using Ninject.Web.WebApi;
using Ninject.Web.WebApi.Filter;


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
