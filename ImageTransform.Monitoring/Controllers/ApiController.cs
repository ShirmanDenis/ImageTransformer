using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImageTransform.Client;
using ImageTransform.Client.Models;
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
        public async Task<OperationResult<IEnumerable<string>>> GetFilters()
        {
            return await _imageTransformClient.GetFiltersAsync().ConfigureAwait(false);
        }

        [HttpPost]
        [Route("filtrate")]
        public async Task<OperationResult<string>> Filtrate([FromBody] FiltrateImageModel filtrateImageModel)
        {
            if (!ModelState.IsValid)
                return OperationResult<string>.CreateFailed("Request model is not valid.");

            var operationResult = await _imageTransformClient
                .FiltrateImageAsync(filtrateImageModel)
                .ConfigureAwait(false);
            return OperationResult<string>.CreateOk(Convert.ToBase64String(operationResult.Result));
        }
    }
}