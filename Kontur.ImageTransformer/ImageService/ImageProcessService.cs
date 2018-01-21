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

        public Bitmap Process(Bitmap image, IImageFilter filter, Rectangle scope, ref bool cancel)
        {
            var cropImage = image.Clone(scope, image.PixelFormat);

            var success = filter.Filtrate(cropImage, ref cancel);
            if (!success)
                throw new OperationCanceledException();

            return cropImage;
        }


        public Rectangle ToCropArea(Size imgSize, int x, int y, int w, int h)
        {
            if (w == 0 || h == 0)
                return Rectangle.Empty;

            var cropArea = Rectangle.Intersect(new Rectangle(new Point(0, 0), imgSize), new Rectangle(x, y, w, h));
            if (cropArea.Width == 0 || cropArea.Height == 0||
                (cropArea.Width == cropArea.Height &&
                cropArea.X == cropArea.Y &&
                cropArea.Width == cropArea.X))
                return Rectangle.Empty;

            return cropArea;
        }
    }
}
