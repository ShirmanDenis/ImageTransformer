using System;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Http.SelfHost;
using FluentAssertions;
using ImageTransformerTests.Properties;
using Kontur.ImageTransformer.Controller;
using Kontur.ImageTransformer.Filters;
using Kontur.ImageTransformer.FiltersFactory;
using Kontur.ImageTransformer.ImageService;
using Kontur.ImageTransformer.ServerConfig;
using Moq;
using NUnit.Framework;

namespace ImageTransformerTests
{
    [TestFixture]
    public class ServerTests
    {
        private const string Prefix = "http://localhost:8080";
        private Stream _imgStream;
      
        private readonly HttpSelfHostServer _server = new HttpSelfHostServer(Config.CreateConfig(Prefix));
        private readonly HttpClient _client = new HttpClient();

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _imgStream = new MemoryStream();
            Resources.TestImg.Save(_imgStream, ImageFormat.Png);
            _imgStream.Position = 0;
        }

        [SetUp]
        public void SetUp()
        {
            _server.OpenAsync();
        }

        [TearDown]
        public void TearDown()
        {
            _server.CloseAsync();
        }

        [Test]
        public async Task Test()
        {
            var response = await _client.PostAsync(Prefix + "/process/sepia/1,1,1,1", new StreamContent(_imgStream));
            var str = await response.Content.ReadAsStringAsync();


        }
    }
}
