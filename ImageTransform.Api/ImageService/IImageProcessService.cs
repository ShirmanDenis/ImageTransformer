using System.Drawing;
using ImageTransform.Api.ImageFilters;

namespace ImageTransform.Api.ImageService
{
    public interface IImageProcessService
    {
        ImageServiceOptions Options { get; set; }

        Bitmap Process(Bitmap bitmap, Rectangle scope, IImageFilter filter, params object[] filterParams);

        Rectangle ToCropArea(Size imgSize, int x, int y, int w, int h);
    }
}
