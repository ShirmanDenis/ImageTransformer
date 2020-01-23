using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text.RegularExpressions;
using ImageTransform.Api.FiltersFactory;

namespace ImageTransform.Api.ImageFilters
{
    public class FilterByRouteResolver : IFilterByRouteResolver, IParamsFromRouteExtractor
    {
        private readonly IFiltersFactory _factory;
        private readonly ConcurrentBag<Regex> _routeValidators = new ConcurrentBag<Regex>();
        public FilterByRouteResolver(IFiltersFactory factory)
        {
            _factory = factory;
        }

        public void AddRouteValidator(string pattern)
        {
            var regex = new Regex(pattern);
            if (!regex.GetGroupNames().Contains("FilterName"))
                throw new FormatException("Pattern must contain a group with name \"FilterName\"");
            _routeValidators.Add(regex);
        }

        public IImageFilter Resolve(string route)
        {
            foreach (var regex in _routeValidators)
            {
                var match = regex.Match(route);
                if (!match.Success) continue;

                var filterName = match.Groups["FilterName"].Value;

                return _factory.GetFilter(filterName);
            }

            return null;
        }

        public object[] GetParams(string filterRoute)
        {
            foreach (var routeValidator in _routeValidators)
            {
                
                var match = routeValidator.Match(filterRoute);
                if (!match.Success) continue;

                var @params = match.Groups["params"];

                if (@params.Success)
                    return @params.Captures
                        .Select(capture => capture.Value)
                        .Cast<object>()
                        .ToArray();
            }
            return null;
        }
    }
}
