using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kontur.ImageTransformer.Filters;
using Kontur.ImageTransformer.FiltersFactory;

namespace Kontur.ImageTransformer.ImageService
{
    public class ImageProcessService : IImageProcessService
    {
        public IImageServiceOptions ServiceOptions { get; set; }

        public ImageProcessService(IImageServiceOptions serviceOptions)
        {
            ServiceOptions = serviceOptions;
        }

        public Bitmap Process(Bitmap image, IImageFilter filter, Rectangle scope)
        {
            var cropImage = image.Clone(scope, image.PixelFormat);

            return filter.Filtrate(cropImage);
        }

        public Rectangle ToCropArea(Bitmap bitmap, int x, int y, int w, int h)
        {
            return new Rectangle(0,0, bitmap.Width, bitmap.Height);
        }
    }
}
