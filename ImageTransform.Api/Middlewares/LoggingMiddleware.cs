using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Vostok.Logging.Abstractions;

namespace Kontur.ImageTransformer.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ILog log)
        {
            log.Info($"Invoking {context.Request.Path}");
            await _next(context);
            log.Info($"Invoked {context.Request.Path}");
        }
    }
}