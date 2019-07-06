using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Kontur.ImageTransformer.ImageFilters;
using Kontur.ImageTransformer.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Kontur.ImageTransformer.ModelBinders
{
    public class FilterModelBinder : IModelBinder
    {
        private readonly IParamsFromRouteExtractor _paramsFromRouteExtractor;

        public FilterModelBinder(IParamsFromRouteExtractor paramsFromRouteExtractor)
        {
            _paramsFromRouteExtractor = paramsFromRouteExtractor;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var path = bindingContext.HttpContext.Request.Path;
            var regex = new Regex(@"/(?<x>[-]?\d+),(?<y>[-]?\d+),(?<w>[-]?\d+),(?<h>[-]?\d+$)");
            var match = regex.Match(path);
            if (!match.Success)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }

            var filterModel = new FilterModel
            {
                X = int.Parse(match.Groups["x"].Value),
                Y = int.Parse(match.Groups["y"].Value),
                H = int.Parse(match.Groups["h"].Value),
                W = int.Parse(match.Groups["w"].Value),
                FilterParams = _paramsFromRouteExtractor.GetParams(path)
            };

            bindingContext.Result = ModelBindingResult.Success(filterModel);

            return Task.CompletedTask;
        }
    }
}
