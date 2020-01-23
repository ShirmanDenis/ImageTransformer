using JetBrains.Annotations;

namespace Kontur.ImageTransformer.ImageFilters
{
    public interface IFilterByRouteResolver
    {
        IImageFilter Resolve([NotNull] string route);
    }
}