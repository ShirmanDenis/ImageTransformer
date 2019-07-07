using System.Collections.Generic;
using System.Threading.Tasks;
using ImageTransform.Client;
using Microsoft.AspNetCore.Mvc;
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
            _log.Info($"Received request {Request.Path}");
            var operationResult = await _imageTransformClient.GetRegisteredFilters();

            return operationResult.Result;
        }
    }
}