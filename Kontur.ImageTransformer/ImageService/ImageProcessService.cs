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

            filter.Filtrate(cropImage);

            return cropImage;
        }

        public async Task<Bitmap> ProcessAsync(Bitmap image, IImageFilter filter, Rectangle scope)
        {
            var cropImage = image.Clone(scope, image.PixelFormat);

            await filter.FiltrateAsync(cropImage);

            return cropImage;
        }

        public Rectangle ToCropArea(Bitmap bitmap, int x, int y, int w, int h)
        {
            if (bitmap == null)
                throw new ArgumentNullException(nameof(bitmap));

            if(w == 0 || h == 0) 
                return Rectangle.Empty;
            
            return Rectangle.Intersect(new Rectangle(new Point(0, 0), bitmap.Size), new Rectangle(x, y, w, h));
        }
    }
}
