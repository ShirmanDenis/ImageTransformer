using JetBrains.Annotations;

namespace ImageTransform.Api.ImageFilters
{
    public interface IParamsFromRouteExtractor
    {
        [CanBeNull]
        object[] GetParams([NotNull] string filterRoute);
    }
}