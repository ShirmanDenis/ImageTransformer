using System;
using System.Collections.Concurrent;
using System.Linq;
using Kontur.ImageTransformer.ImageFilters;

namespace Kontur.ImageTransformer.FiltersFactory
{
    public class FiltersFactory : IFiltersFactory
    {
        private readonly ConcurrentDictionary<string, IImageFilter> _filtersCache = new ConcurrentDictionary<string, IImageFilter>();

        public void RegisterFilter(string name, IImageFilter filter)
        {
            if (IsRegistered(name))
                throw new Exception($"Filter with name \"{name}\" already registered");

            _filtersCache.AddOrUpdate(name, filter, (s, imageFilter) => imageFilter);
            _filtersCache.TryAdd(name, filter);
        }

        public IImageFilter GetFilter(string name)
        {
            if (!_filtersCache.TryGetValue(name, out var filter))
               throw new Exception($"Filter with name \"{name}\" is not registered");

            return filter;
        }

        public IImageFilter[] GetRegisteredFilters()
        {
            return _filtersCache.Values.ToArray();
        }

        public bool IsRegistered(string name) 
        {
            return _filtersCache.ContainsKey(name);
        }
    }
}
