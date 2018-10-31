using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Kontur.ImageTransformer;
using Kontur.ImageTransformer.ServerConfig;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace ImageTransformerTests
{
    [TestFixture]
    public class ServerTests
    {
        private const string Prefix = "http://localhost:8080";
        private byte[] _imgData;

        private HttpClient _client;
        private readonly WebApplicationFactory<Config> _factory = new WebApplicationFactory<Config>();
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var imgStream = new MemoryStream();
            Resources.AlphaImg.Save(imgStream, ImageFormat.Png);
            imgStream.Position = 0;
            _imgData = imgStream.ToArray();
            _client = _factory.CreateClient();
        }

        [Test]
        [TestCase("/process/sepia/-5,5,20,20")]
        [TestCase("/process/sepia/1,1,50,50")]
        [TestCase("/process/grayscale/-5,5,20,20")]
        [TestCase("/process/grayscale/1,1,50,50")] 
        [TestCase("/process/threshold(0)/-5,5,20,20")]
        [TestCase("/process/threshold(5)/-5,5,20,20")]
        [TestCase("/process/threshold(99)/1,1,50,50")]
        public async Task Server_shouldReturn200OK_WhenCorrectRequestUri(string uri)
        {
            var response = await _client.PostAsync(Prefix + uri, new ByteArrayContent(_imgData));

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.OK);
        }

        [TestCase("/process/sepi/-5,5,20,20")]
        [TestCase("/process/epia/1,1,50,50")]
        [TestCase("/process/spia/40,40,-20,-20")]
        [TestCase("/process/grayscal/-5,5,20,20")]
        [TestCase("/process/gryscale/1,1,50,50")]
        [TestCase("/process/graysale/40,40,-20,-20")]        
        [TestCase("/process/threshold(0z)/-5,5,20,20")]
        [TestCase("/process/thrshold(5)/-5,5,20,20")]
        [TestCase("/process/threshold(9s9)/-5,5,-20,-20")]
        [TestCase("/process/threshold(a99)/1,1,50,50")]
        [TestCase("/process/threshold55/40,40,-20,-20")]
        public async Task Server_shouldReturn400BadRequest_whenRequestWithUnknownFilterName(string uri)
        {
            var response = await _client.PostAsync(Prefix + uri, new ByteArrayContent(_imgData));

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.BadRequest);
        }

        [Test]
        [TestCase("/process/sepia/-5,5a,20,20")]
        [TestCase("/process/sepia/1,,50,50")]
        [TestCase("/process/sepia/40,-20,-20")]
        [TestCase("/process/grayscale/-5 5 20 20")]
        [TestCase("/process/grayscale/1, 1,50,50")]
        [TestCase("/process/grayscale/40, 40, -20, -20")]        
        [TestCase("/process/threshold(0)/")]
        [TestCase("/process/threshold(5)/-5")]
        public async Task Server_shouldReturn400BadRequest_whenRequestUriWithIncorrectCoords(string uri)
        {
            var response = await _client.PostAsync(Prefix + uri, new ByteArrayContent(_imgData));

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.BadRequest);
        }

        [Test]
        [TestCase("/process/sepia/0,0,0,0")]
        [TestCase("/process/sepia/1,1,1,1")]
        [TestCase("/process/sepia/271,304,271,304")]
        [TestCase("/process/sepia/271,304,0,0")]
        [TestCase("/process/sepia/0,0,-271,-304")]
        [TestCase("/process/grayscale/0,0,0,0")]
        [TestCase("/process/grayscale/271,304,271,304")]
        [TestCase("/process/grayscale/271,304,0,0")]
        [TestCase("/process/grayscale/0,0,-271,-304")]
        [TestCase("/process/threshold(0)/0,0,0,0")]
        [TestCase("/process/threshold(0)/271,304,271,304")]
        [TestCase("/process/threshold(0)/271,304,0,0")]
        [TestCase("/process/threshold(0)/0,0,-271,-304")]
        public async Task Server_shouldReturn204NoContent_whenIntersectionOfTheRectangleWithTheImageIsEmpty(string uri)
        {
            var response = await _client.PostAsync(Prefix + uri, new ByteArrayContent(_imgData));

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.NoContent);
        }

        [Test]
        [TestCase("/process/sepia/-5,5,20,20")]
        [TestCase("/process/sepia/1,1,50,50")]
        [TestCase("/process/sepia/40,40,-20,-20")]
        [TestCase("/process/grayscale/-5,5,20,20")]
        [TestCase("/process/grayscale/1,1,50,50")]
        [TestCase("/process/grayscale/40,40,-20,-20")]        
        [TestCase("/process/threshold(0)/-5,5,20,20")]
        [TestCase("/process/threshold(5)/-5,5,20,20")]
        [TestCase("/process/threshold(99)/-5,5,-20,-20")]
        [TestCase("/process/threshold(99)/1,1,50,50")]
        [TestCase("/process/threshold(55)/40,40,-20,-20")]
        public async Task Server_shouldReturn400BadRequest_whenRequestContentLength_greaterThan100KB(string uri)
        {
            var imgStream = new MemoryStream();
            Resources.BigImage.Save(imgStream, ImageFormat.Png);
            imgStream.Position = 0;
            var response = await _client.PostAsync(Prefix + uri, new StreamContent(imgStream));

            response.StatusCode.Should().BeEquivalentTo(HttpStatusCode.BadRequest);
        }
    }
}
