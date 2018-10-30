using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kontur.ImageTransformer.ImageFilters;

namespace Kontur.ImageTransformer.FiltersFactory
{
    public class FiltersFactory : IFiltersFactory
    {
        private readonly Dictionary<string, IImageFilter> _filtersCache = new Dictionary<string, IImageFilter>();

        public void RegisterFilter(string name, IImageFilter filter)
        {
            if (IsRegistered(name))
                throw new Exception($"Filter with name \"{name}\" already registered");

            _filtersCache.Add(name, filter);
        }

        public IImageFilter GetFilter(string name)
        {
            if (!_filtersCache.TryGetValue(name, out IImageFilter filter))
               throw new Exception($"Filter with name \"{name}\" is not registered");

            return filter;
        }

        public bool IsRegistered(string name) 
        {
            return _filtersCache.ContainsKey(name);
        }
    }
}
