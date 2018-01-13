using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using NLog;
using NLog.Fluent;

namespace Kontur.ImageTransformer.ServerConfig
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public override Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            _logger.Error(context.Exception);

            return base.HandleAsync(context, cancellationToken);
        }
    }
}
