namespace Kontur.ImageTransformer.Filters
{
    public interface IFilterByRouteResolver
    {
        IImageFilter Resolve(string route);
    }
}