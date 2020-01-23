using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ImageTransform.Client;
using ImageTransform.Monitoring.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
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
        public async Task<OperationResult<IEnumerable<string>>> GetFilters()
        {
            var operationResult = await _imageTransformClient.GetRegisteredFilters();

            return operationResult;
        }

        [HttpPost]
        [Route("filtrate")]
        public async Task<string> Filtrate([FromBody]FiltrateImageModel filtrateImageModel)
        {
            var operationResult = await _imageTransformClient
                .FiltrateImage(filtrateImageModel.ImgData, filtrateImageModel.FilterName, new Rectangle(0,0, 100, 100));
            return Convert.ToBase64String(operationResult.Result);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _log.Info($"Received request {Request.Path}");
            base.OnActionExecuting(context);
        }
    }
}