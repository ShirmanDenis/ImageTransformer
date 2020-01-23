using ImageTransform.Core.ImageFilters;
using JetBrains.Annotations;

namespace ImageTransform.Core.FiltersFactory
{
    public interface IFiltersFactory
    {
        void RegisterFilter([NotNull] string name, [NotNull] IImageFilter filter);
        IImageFilter GetFilter([NotNull] string name);

        [NotNull]
        IImageFilter[] GetRegisteredFilters();
        bool IsRegistered([NotNull] string name);
    }
}