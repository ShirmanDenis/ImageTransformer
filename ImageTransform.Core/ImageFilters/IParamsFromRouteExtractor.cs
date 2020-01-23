using JetBrains.Annotations;

namespace ImageTransform.Core.ImageFilters
{
    public interface IParamsFromRouteExtractor
    {
        [CanBeNull]
        object[] GetParams([NotNull] string filterRoute);
    }
}