using System.Net;
using Kontur.ImageTransformer.ImageService;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Kontur.ImageTransformer.ActionFilters
{
    public class ImageSizeLimit : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = (Microsoft.AspNetCore.Mvc.Controller)context.Controller;
            var options = controller.HttpContext.RequestServices.GetService<ImageServiceOptions>();
            var request = controller.Request;
            if (request.ContentLength == null || request.ContentLength > options.MaxImageSize || request.ContentLength == 0)
            {
                context.Result = controller.BadRequest();
                return;
            }
            base.OnActionExecuting(context);
        }
    }
}
