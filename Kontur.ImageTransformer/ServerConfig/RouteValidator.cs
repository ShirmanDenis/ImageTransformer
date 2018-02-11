using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;

namespace Kontur.ImageTransformer.ServerConfig
{
    public class RouteValidator : DelegatingHandler
    {
        private readonly Regex _uriRegex = 
            new Regex(@"^/process/((sepia)|(grayscale)|(threshold\(([1-9][0-9]?)\))|(threshold\(0\))|(threshold\(100\)))/[-]?\d+,[-]?\d+,[-]?\d+,[-]?\d+$");

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {         
            if (request.Method == HttpMethod.Post &&
                _uriRegex.IsMatch(request.RequestUri.AbsolutePath))
            {                    
                return await base.SendAsync(request, cancellationToken);
            }

            return request.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}
