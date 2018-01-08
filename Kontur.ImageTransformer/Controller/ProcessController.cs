using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Web.Http.Results;
using Kontur.ImageTransformer.Filters;
using Kontur.ImageTransformer.FiltersFactory;
using Kontur.ImageTransformer.ImageService;
using Ninject;

namespace Kontur.ImageTransformer.Controller
{
    [RoutePrefix("process")]
    public class ProcessController : ApiController
    {
        private readonly IImageProcessService _service;
        private readonly IFiltersFactory _filtersFactory;

        public ProcessController(IImageProcessService service, IFiltersFactory filtersFactory)
        {
            _service = service;
            _filtersFactory = filtersFactory;
        }

        [Route("threshold({value})/{x},{y},{w},{h}")]
        public async Task<IHttpActionResult> PostThreshold(int value, int x, int y, int w, int h)
        {
            var filter = _filtersFactory.GetFilter("threshold");
            var filterParam = filter.AddParam();
            filterParam.Value = value;

            return await ProcessAndSendAsync(filter, x, y, w, h);
        }

        [Route("sepia/{x},{y},{w},{h}")]
        public async Task<IHttpActionResult> PostSepia(int x, int y, int w, int h)
        {
            var filter = _filtersFactory.GetFilter("sepia");
            
            return await ProcessAndSendAsync(filter, x, y, w, h);
        }

        [Route("grayscale/{x},{y},{w},{h}")]
        public async Task<IHttpActionResult> PostGrayscale(int x, int y, int w, int h)
        {
            var filter = _filtersFactory.GetFilter("grayscale");

            return await ProcessAndSendAsync(filter, x, y, w, h);
        }

        private async Task<IHttpActionResult> ProcessAndSendAsync(IImageFilter filter, int x, int y, int w, int h)
        {
            using (var imageStream = await ControllerContext.Request.Content.ReadAsStreamAsync())
            {
                Bitmap responseImage;
                using (var imageFromRequest = new Bitmap(imageStream))
                {
                    var cropArea = _service.ToCropArea(imageFromRequest, x, y, w, h);
                    responseImage = _service.Process(imageFromRequest, filter, cropArea);
                }
                using (var responseStream = new MemoryStream())
                {
                    responseImage.Save(responseStream, ImageFormat.Png);
                    using (var response = ControllerContext.Request.CreateResponse(HttpStatusCode.OK,
                        new StreamContent(responseStream)))
                    {                       
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        return ResponseMessage(response);
                    }
                }
            }
        }

        public override Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
            var contentLength = controllerContext.Request.Content.Headers.ContentLength;
            if (contentLength == null || contentLength > _service.ServiceOptions.MaxImageSize)
                return Task.Factory.StartNew(() => new HttpResponseMessage(HttpStatusCode.BadRequest), cancellationToken);

            return base.ExecuteAsync(controllerContext, cancellationToken);
        } 
    }
}
