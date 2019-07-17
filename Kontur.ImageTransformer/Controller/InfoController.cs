using System.Collections.Generic;
using System.Linq;
using Kontur.ImageTransformer.FiltersFactory;
using Microsoft.AspNetCore.Mvc;

namespace Kontur.ImageTransformer.Controller
{
    [Route("info")]
    public class InfoController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IFiltersFactory _filtersFactory;

        public InfoController(IFiltersFactory filtersFactory)
        {
            _filtersFactory = filtersFactory;
        }

        [HttpGet]
        [Route("defined/filters")]
        public IEnumerable<string> GetDefinedFilters()
        {
            return _filtersFactory.GetRegisteredFilters().Select(f => f.Name.ToLower());
        }
    }
}