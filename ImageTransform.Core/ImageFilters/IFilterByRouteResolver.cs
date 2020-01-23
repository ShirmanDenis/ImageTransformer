using JetBrains.Annotations;

namespace ImageTransform.Core.ImageFilters
{
    public interface IFilterByRouteResolver
    {
        IImageFilter Resolve([NotNull] string route);
    }
}