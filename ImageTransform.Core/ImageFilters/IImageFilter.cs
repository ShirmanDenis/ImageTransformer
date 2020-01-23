using System.Drawing;

namespace ImageTransform.Core.ImageFilters
{
    public interface IImageFilter
    {
        string Name { get; }
        bool Filtrate(Bitmap bitmap, params object[] parameters);
    }
}