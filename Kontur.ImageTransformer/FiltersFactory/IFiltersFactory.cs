using JetBrains.Annotations;
using Kontur.ImageTransformer.ImageFilters;

namespace Kontur.ImageTransformer.FiltersFactory
{
    public interface IFiltersFactory
    {
        void RegisterFilter([NotNull] string name, [NotNull] IImageFilter filter);
        IImageFilter GetFilter([NotNull] string name);
        bool IsRegistered([NotNull] string name);
    }
}