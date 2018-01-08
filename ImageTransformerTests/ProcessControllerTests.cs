using System;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using FluentAssertions;
using ImageTransformerTests.Properties;
using Kontur.ImageTransformer.Controller;
using Kontur.ImageTransformer.FiltersFactory;
using Kontur.ImageTransformer.ImageService;
using Moq;
using NUnit.Framework;

namespace ImageTransformerTests
{
    [TestFixture]
    public class ProcessControllerTests
    {
        private const string Prefix = "http://localhost:8080/";
        private ProcessController _controller;
        private Stream _imgStream;

        private Mock<IImageProcessService> _mockService;
        private Mock<IFiltersFactory> _mockFactory;

        [SetUp]
        public void InitTestMethod()
        {
            _imgStream = new MemoryStream();
            Resources.TestImg.Save(_imgStream, ImageFormat.Png);
            _mockService = new Mock<IImageProcessService>();
        }

        [Test]
        [TestCase( 0,  0, 20, 20)]
        [TestCase(-1, -1, 50, 50)]
        [TestCase(-1,  1, 20, 20)]
        [TestCase( 1, -1, 20, 20)]
        [TestCase(50, 80, -2, -2)]
        [TestCase(50, 80, 10, -1)]
        public async Task CorrectPostGrayscale_shouldReturn_200Ok(int x, int y, int w, int h)
        {
            var request =
                new HttpRequestMessage(HttpMethod.Post, Prefix + $"process/grayscale/{x},{y},{w},{h}")
                {
                    Content = new StreamContent(_imgStream)
                };
            _controller = new ProcessController(_mockService.Object, _mockFactory.Object) {Request = request};

            var result = await _controller.PostGrayscale(x, y, w, h);
            
            result.Should().BeOfType<OkResult>();
        }

        [Test]
        [TestCase( 0,  0, 20, 20)]
        [TestCase(-1, -1, 50, 50)]
        [TestCase(-1,  1, 20, 20)]
        [TestCase( 1, -1, 20, 20)]
        [TestCase(50, 80, -2, -2)]
        [TestCase(50, 80, 10, -1)]
        public async Task CorrectPostSepia_shouldReturn_200Ok(int x, int y, int w, int h)
        {
            var request =
                new HttpRequestMessage(HttpMethod.Post, Prefix + $"process/sepia/{x},{y},{w},{h}")
                {
                    Content = new StreamContent(_imgStream)
                };
            _controller = new ProcessController(_mockService.Object, _mockFactory.Object) { Request = request };

            var result = await _controller.PostSepia(x, y, w, h);

            result.Should().BeOfType<OkResult>();
        }

        [Test]
        [TestCase( 0,  0, 20, 20)]
        [TestCase(-1, -1, 50, 50)]
        [TestCase(-1,  1, 20, 20)]
        [TestCase( 1, -1, 20, 20)]
        [TestCase(50, 80, -2, -2)]
        [TestCase(50, 80, 10, -1)]
        public async Task CorrectPostThreshold_shouldReturn_200Ok(int x, int y, int w, int h)
        {
            var request =
                new HttpRequestMessage(HttpMethod.Post, Prefix + $"process/threshold({5})/{x},{y},{w},{h}")
                {
                    Content = new StreamContent(_imgStream)
                };
            _controller = new ProcessController(_mockService.Object, _mockFactory.Object) { Request = request };

            var result = await _controller.PostGrayscale(x, y, w, h);

            result.Should().BeOfType<OkResult>();
        }

        [Test]
        public async Task Controller_shouldReturn_400BadRequest_whenImageBiggerThan100kb()
        {
            _mockService.SetupGet(service => service.ServiceOptions.MaxImageSize).Returns(100 * 1024);
            var stream = new MemoryStream();
            Resources.BigImage.Save(stream, ImageFormat.Png);
            var request =
                new HttpRequestMessage(HttpMethod.Post, Prefix)
                {
                    Content = new StreamContent(_imgStream)
                };
            request.Content.Headers.ContentLength = Resources.BigImage.Height * Resources.BigImage.Width;
            _controller = new ProcessController(_mockService.Object, _mockFactory.Object) { Request = request };

            var result = await _controller.ExecuteAsync(_controller.ControllerContext, CancellationToken.None);

            result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task Controller_shouldReturn_400BadRequest_whenIncorrectUri()
        {
            _mockService.SetupGet(service => service.ServiceOptions.MaxImageSize).Returns(100 * 1024);
            var request =
                new HttpRequestMessage(HttpMethod.Post, Prefix + "process/threshold/")
                {
                    Content = new StreamContent(_imgStream)
                };
            _controller = new ProcessController(_mockService.Object, _mockFactory.Object) { Request = request };

            var result = await _controller.ExecuteAsync(_controller.ControllerContext, CancellationToken.None);

            result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.BadRequest);
        }
    }
}
