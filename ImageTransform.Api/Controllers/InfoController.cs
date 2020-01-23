using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ImageTransform.Core.FiltersFactory;

namespace ImageTransform.Api.Controllers
{
    [Route("info")]
    public class InfoController : Controller
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