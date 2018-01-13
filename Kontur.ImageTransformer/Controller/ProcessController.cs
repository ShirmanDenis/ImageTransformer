﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Kontur.ImageTransformer.Controller
{
    [RoutePrefix("process")]
    public class ProcessController : ApiController
    {
        private const string StrThreshold = "threshold";
        private const string StrSepia = "sepia";
        private const string StrGrayscale = "grayscale";
        private const string ContentType = "image/png";

        private readonly IImageProcessService _service;
        private readonly IFiltersFactory _filtersFactory;

        public ProcessController(IImageProcessService service, IFiltersFactory filtersFactory)
        {
            _service = service;
            _filtersFactory = filtersFactory;
        }
        [Route("hello")]
        public string GetSomeData()
        {
            return "Hello motherfucker!";
        }

        [Route("threshold({value})/{x},{y},{w},{h}")]
        public async Task<HttpResponseMessage> PostThreshold(int value, int x, int y, int w, int h)
        {
            var filter = _filtersFactory.GetFilter(StrThreshold);
            filter.AddParam(value);

            return await ProcessAndSendAsync(filter, x, y, w, h);
        }

        [Route("sepia/{x},{y},{w},{h}")]
        public async Task<HttpResponseMessage> PostSepia(int x, int y, int w, int h)
        {
            var filter = _filtersFactory.GetFilter(StrSepia);
            
            return await ProcessAndSendAsync(filter, x, y, w, h);
        }

        [Route("grayscale/{x},{y},{w},{h}")]
        public async Task<HttpResponseMessage> PostGrayscale(int x, int y, int w, int h)
        {
            var filter = _filtersFactory.GetFilter(StrGrayscale);

            return await ProcessAndSendAsync(filter, x, y, w, h);
        }

        private async Task<HttpResponseMessage> ProcessAndSendAsync(IImageFilter filter, int x, int y, int w, int h)
        {
            using (var requestStream = await Request.Content.ReadAsStreamAsync())
            using (var imgFromRequest = new Bitmap(requestStream))
            {
                var cropArea = _service.ToCropArea(imgFromRequest, x, y, w, h);
                var response = Request.CreateResponse();
                var responseImgStream = new MemoryStream();

                await _service.ProcessAsync(imgFromRequest, filter, cropArea);
                imgFromRequest.Save(responseImgStream, ImageFormat.Png);

                responseImgStream.Position = 0;
                response.Content = new StreamContent(responseImgStream);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue(ContentType);
                response.Content.Headers.ContentLength = responseImgStream.Length;
                return response;
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
