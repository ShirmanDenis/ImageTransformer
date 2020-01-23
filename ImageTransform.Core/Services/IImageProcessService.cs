using System.Drawing;
using ImageTransform.Core.ImageFilters;

namespace ImageTransform.Core.Services
{
    public interface IImageProcessService
    {

        Bitmap Process(Bitmap bitmap, Rectangle scope, IImageFilter filter, params object[] filterParams);

        Rectangle ToCropArea(Size imgSize, int x, int y, int w, int h);
    }
}
