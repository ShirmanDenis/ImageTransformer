using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Kontur.ImageTransformer.ActionFilters;
using Kontur.ImageTransformer.ImageFilters;
using Kontur.ImageTransformer.FiltersFactory;
using Kontur.ImageTransformer.ImageService;
using Kontur.ImageTransformer.ModelBinders;
using Kontur.ImageTransformer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Kontur.ImageTransformer.Controller
{
    [Route("process")]
    public class ProcessController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IImageProcessService _service;
        private readonly IFilterByRouteResolver _filterResolver;

        public ProcessController(
            IImageProcessService service, 
            IFilterByRouteResolver filterResolver)
        {
            _service = service;
            _filterResolver = filterResolver;
        }

        [HttpPost]
        [Route("{*route}")]
        [ImageSizeLimit]
        public IActionResult Process(string route, FilterModel filterModel)
        {
            var filter = _filterResolver.Resolve(route);
            if (filter == null || filterModel == null)
                return BadRequest();

            return ProcessAndSend(filter, filterModel.X, filterModel.Y, filterModel.W, filterModel.H, filterModel.FilterParams);
        }

        private IActionResult ProcessAndSend(IImageFilter filter, int x, int y, int w, int h, params object[] filterParams)
        {
            IActionResult result;
            try
            {
                using (var imgFromRequest = new Bitmap(Request.Body))
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
                result = StatusCode(500);
            }
            return result;
        }
    }
}
