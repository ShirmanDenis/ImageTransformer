namespace Kontur.ImageTransformer.ImageFilters
{
    public interface IFilterByRouteResolver
    {
        IImageFilter Resolve(string route);
    }
}