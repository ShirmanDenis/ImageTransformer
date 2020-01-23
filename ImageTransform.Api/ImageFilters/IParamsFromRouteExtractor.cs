using JetBrains.Annotations;

namespace Kontur.ImageTransformer.ImageFilters
{
    public interface IParamsFromRouteExtractor
    {
        [CanBeNull]
        object[] GetParams([NotNull] string filterRoute);
    }
}