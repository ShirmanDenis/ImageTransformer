﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.ImageTransformer.Filters
{
    public class GrayscaleFilter : IImageFilter
    {
        [DllImport("ImgProc.dll")]
        private static extern void Grayscale(IntPtr imgData, int height, int width, int bytesPerPixel, int stride);

        public IImageFilterParam[] Params { get; } = new IImageFilterParam[0];

        public Bitmap Filtrate(Bitmap img)
        {
            var bitmapData = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadWrite, img.PixelFormat);
            var bytesPerPixel = Image.GetPixelFormatSize(img.PixelFormat) / 8;

            Grayscale(bitmapData.Scan0, img.Height, img.Width, bytesPerPixel, bitmapData.Stride);

            return img;
        }

        public IImageFilterParam AddParam()
        {
            // Nothing to do here
            return null;
        }
    }
}
