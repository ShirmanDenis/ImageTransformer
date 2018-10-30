﻿using Kontur.ImageTransformer.ImageFilters;

namespace Kontur.ImageTransformer.FiltersFactory
{
    public interface IFiltersFactory
    {
        void RegisterFilter(string name, IImageFilter filter);
        IImageFilter GetFilter(string name);
        bool IsRegistered(string name);
    }
}