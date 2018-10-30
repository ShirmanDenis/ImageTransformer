using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Kontur.ImageTransformer.FiltersFactory;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Kontur.ImageTransformer.ImageFilters
{
    public class FilterByRouteResolver : IFilterByRouteResolver
    {
        private readonly IFiltersFactory _factory;
        private readonly List<Regex> _routeValidators = new List<Regex>();
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
    }
}
