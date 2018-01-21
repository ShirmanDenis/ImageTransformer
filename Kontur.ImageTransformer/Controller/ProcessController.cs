using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Http.Controllers;
using Kontur.ImageTransformer.Common;
using Kontur.ImageTransformer.Filters;
using Kontur.ImageTransformer.FiltersFactory;
using Kontur.ImageTransformer.ImageService;
using NLog;

namespace Kontur.ImageTransformer.Controller
{
    [RoutePrefix("process")]
    public class ProcessController : ApiController
    {
        private readonly IImageProcessService _service;
        private readonly IFiltersFactory _filtersFactory;

        private bool _cancel = false;

        public ProcessController(IImageProcessService service, IFiltersFactory filtersFactory)
        {
            _service = service;
            _filtersFactory = filtersFactory;
        }

        [Route("threshold({value})/{x},{y},{w},{h}")]
        public async Task<IHttpActionResult> PostThreshold(int value, int x, int y, int w, int h)
        {
            var filter = _filtersFactory.GetFilter(Constants.StrThreshold);
            filter.AddParam(value);

            return await ProcessAndSendAsync(filter, x, y, w, h);
        }

        [Route("sepia/{x},{y},{w},{h}")]
        public async Task<IHttpActionResult> PostSepia(int x, int y, int w, int h)
        {
            var filter = _filtersFactory.GetFilter(Constants.StrSepia);

            return await ProcessAndSendAsync(filter, x, y, w, h);
        }

        [Route("grayscale/{x},{y},{w},{h}")]
        public async Task<IHttpActionResult> PostGrayscale(int x, int y, int w, int h)
        {
            var filter = _filtersFactory.GetFilter(Constants.StrGrayscale);

            return await ProcessAndSendAsync(filter, x, y, w, h);
        }
        
        private async Task<IHttpActionResult> ProcessAndSendAsync(IImageFilter filter, int x, int y, int w, int h)
        {
            ResponseMessageResult result;
            try
            {
                using (var requestStream = await Request.Content.ReadAsStreamAsync())
                using (var imgFromRequest = new Bitmap(requestStream))
                {
                    var cropArea = _service.ToCropArea(imgFromRequest.Size, x, y, w, h);
                    if (cropArea == Rectangle.Empty)
                    {
                        return ResponseMessage(Request.CreateResponse(HttpStatusCode.NoContent));
                    }

                    var responseImgStream = new MemoryStream();
                    using (var processedImg = _service.Process(imgFromRequest, filter, cropArea, ref _cancel))
                        processedImg.Save(responseImgStream, ImageFormat.Png);
                    responseImgStream.Position = 0;

                    var response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StreamContent(responseImgStream);
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue(Constants.ContentType);

                    result = ResponseMessage(response);
                }
            }
            catch (OperationCanceledException)
            {
                result = ResponseMessage(Request.CreateResponse(429));
            }
            catch (Exception e)
            {
                LogManager.GetCurrentClassLogger().Error(e);
                result = ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e));
            }
            return result;
        }

        public override async Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
            var contentLength = controllerContext.Request.Content.Headers.ContentLength;
            if (contentLength == null || contentLength > _service.ServiceOptions.MaxImageSize)
                return await Task.Factory.StartNew(() => new HttpResponseMessage(HttpStatusCode.BadRequest), cancellationToken);
            
            var cancelSource = new CancellationTokenSource(1000);
            cancelSource.Token.Register(() => _cancel = true);

            return await base.ExecuteAsync(controllerContext, cancellationToken);
        }
    }
}
