using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Kontur.ImageTransformer.ServerConfig
{
    public class RouteValidator : DelegatingHandler
    {
        private readonly Regex _uriRegex = new Regex(@"^/process/(sepia)|(grayscale)|(threshold\(\d+\))/\d+,\d+,\d+,\d+$");

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Method == HttpMethod.Post &&
                _uriRegex.IsMatch(request.RequestUri.AbsolutePath))
                return base.SendAsync(request, cancellationToken);

            return Task.Factory.StartNew(() => request.CreateResponse(HttpStatusCode.BadRequest), cancellationToken);
        }
    }
}
