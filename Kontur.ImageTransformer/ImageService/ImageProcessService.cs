﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
        public ImageServiceOptions Options { get; set; }

        public ImageProcessService(ImageServiceOptions serviceOptions = null)
        {
            Options = serviceOptions ?? new ImageServiceOptions();
        }

        public Bitmap Process(Bitmap image, IImageFilter filter, Rectangle scope)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));
            try
            {
                var cropImage = image.Clone(scope, image.PixelFormat);
                var success = filter.Filtrate(cropImage);
                if (!success)
                    throw new OperationCanceledException();

                return cropImage;
            }
            catch (Exception e)
            {
                Console.WriteLine(scope);
                Console.WriteLine(e);
                throw;
            }
        }


        public Rectangle ToCropArea(Size imgSize, int x, int y, int w, int h)
        {
            if (w == 0 || h == 0)
                return Rectangle.Empty;
            if (w < 0)
            {
                var temp = x;
                x = Math.Max(x + w, 0);
                w = temp - x;
            }
            if (h < 0)
            {
                var temp = y;
                y = Math.Max(y + h, 0);
                h = temp - y;
            }

            var cropArea = Rectangle.Intersect(new Rectangle(new Point(0, 0), imgSize), new Rectangle(x, y, w, h));
            
            if (cropArea.Width == 0 || 
                cropArea.Height == 0 ||
                cropArea.Width == cropArea.Height &&
                cropArea.X == cropArea.Y &&
                cropArea.Width == cropArea.X)
                return Rectangle.Empty;

            return cropArea;
        }
    }
}
