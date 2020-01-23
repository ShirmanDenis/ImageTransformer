using System.Drawing;

namespace ImageTransform.Api.ImageFilters
{
    public interface IImageFilter
    {
        string Name { get; }
        bool Filtrate(Bitmap bitmap, params object[] parameters);
    }
}