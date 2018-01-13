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

        public void Process(Bitmap image, IImageFilter filter, Rectangle scope)
        {
            var cropImage = image.Clone(scope, image.PixelFormat);

            filter.Filtrate(cropImage);
        }

        public async Task ProcessAsync(Bitmap image, IImageFilter filter, Rectangle scope)
        {
            var cropImage = image.Clone(scope, image.PixelFormat);

            await filter.FiltrateAsync(cropImage);
        }

        public Rectangle ToCropArea(Bitmap bitmap, int x, int y, int w, int h)
        {
            return new Rectangle(0,0, bitmap.Width, bitmap.Height);
        }
    }
}
