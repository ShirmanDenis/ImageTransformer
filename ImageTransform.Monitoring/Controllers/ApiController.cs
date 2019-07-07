using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ImageTransform.Client;
using ImageTransform.Monitoring.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Vostok.Logging.Abstractions;

namespace ImageTransform.Monitoring.Controllers
{
    [Route("api")]
    public class ApiController : Controller
    {
        private readonly IImageTransformClient _imageTransformClient;
        private readonly ILog _log;

        public ApiController(IImageTransformClient imageTransformClient, ILog log)
        {
            _imageTransformClient = imageTransformClient;
            _log = log;
        }

        [HttpGet]
        [Route("filters")]
        public async Task<IEnumerable<string>> GetFilters()
        {
            var operationResult = await _imageTransformClient.GetRegisteredFilters();

            return operationResult.Result;
        }

        [HttpPost]
        [Route("filtrate")]
        public async Task<byte[]> Filtrate([FromBody] FiltrateImageModel filtrateImageModel)
        {
            //var reader = new StreamReader(Request.Body);
            //var s = reader.ReadToEnd();
            var bytes = Encoding.UTF8.GetBytes(filtrateImageModel.ImgData);
            var operationResult = await _imageTransformClient
                .FiltrateImage(bytes, filtrateImageModel.FilterName.ToLower(), new Rectangle(0,0, 100, 100));
            return operationResult.Result;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _log.Info($"Received request {Request.Path}");
            base.OnActionExecuting(context);
        }
    }
}