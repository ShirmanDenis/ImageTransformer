using ImageTransform.Api.ImageFilters;
using JetBrains.Annotations;

namespace ImageTransform.Api.FiltersFactory
{
    public interface IFiltersFactory
    {
        void RegisterFilter([NotNull] string name, [NotNull] IImageFilter filter);
        IImageFilter GetFilter([NotNull] string name);

        IImageFilter[] GetRegisteredFilters();
        bool IsRegistered([NotNull] string name);
    }
}