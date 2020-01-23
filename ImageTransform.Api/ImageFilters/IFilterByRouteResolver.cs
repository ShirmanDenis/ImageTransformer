using JetBrains.Annotations;

namespace ImageTransform.Api.ImageFilters
{
    public interface IFilterByRouteResolver
    {
        IImageFilter Resolve([NotNull] string route);
    }
}