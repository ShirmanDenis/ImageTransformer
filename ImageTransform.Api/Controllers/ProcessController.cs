using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ImageTransform.Api.Models;
using ImageTransform.Core.ImageFilters;
using ImageTransform.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Vostok.Logging.Abstractions;

namespace ImageTransform.Api.Controllers
{
    [Route("process")]
    public class ProcessController : ControllerBase
    {
        private readonly ILog _log;
        private readonly IImageProcessService _service;
        private readonly IFilterByRouteResolver _filterResolver;

        public ProcessController(
            IImageProcessService service,
            IFilterByRouteResolver filterResolver, ILog log)
        {
            _service = service;
            _filterResolver = filterResolver;
            _log = log;
        }

        [HttpPost]
        [Route("{*route}")]
        public IActionResult Process(string route, FilterModel filterModel)
        {
            var filter = _filterResolver.Resolve(route);
            if (filter == null)
                return BadRequest("Can't resolve filter name");
            if (filterModel == null)
                return BadRequest("Filter model is null");

            return ProcessAndSend(filter, filterModel.X, filterModel.Y, filterModel.W, filterModel.H,
                filterModel.FilterParams);
        }

        private IActionResult ProcessAndSend(IImageFilter filter, int x, int y, int w, int h,
            params object[] filterParams)
        {
            IActionResult result;
            try
            {
                using (var imgFromRequest = new Bitmap(Request.BodyReader.AsStream(true)))
                {
                    var cropArea = _service.ToCropArea(imgFromRequest.Size, x, y, w, h);
                    if (cropArea == Rectangle.Empty)
                        return NoContent();

                    var responseImgStream = new MemoryStream();
                    using (var processedImg = _service.Process(imgFromRequest, cropArea, filter, filterParams))
                        processedImg.Save(responseImgStream, ImageFormat.Png);
                    responseImgStream.Position = 0;

                    result = File(responseImgStream, "image/png");
                }
            }
            catch (OperationCanceledException)
            {
                result = StatusCode(429);
            }
            catch (Exception e)
            {
                _log.Error(e);
                result = StatusCode(500);
            }

            return result;
        }
    }
}