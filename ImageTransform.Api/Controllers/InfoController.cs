using System.Collections.Generic;
using System.Linq;
using ImageTransform.Api.FiltersFactory;
using Microsoft.AspNetCore.Mvc;

namespace ImageTransform.Api.Controllers
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