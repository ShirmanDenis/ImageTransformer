﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ImageTransform.Core.ImageFilters
{
    public class SepiaFilter : IImageFilter
    {
        [DllImport("ImgProc.dll")]
        private static extern int Sepia(IntPtr imgData, int height, int width, int bytesPerPixel, int stride);

        public string Name { get; } = "Sepia";

        public bool Filtrate(Bitmap img, params object[] parameters)
        {
            if (img == null) throw new ArgumentNullException(nameof(img));

            var bitmapData = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadWrite, img.PixelFormat);
            var bytesPerPixel = Image.GetPixelFormatSize(img.PixelFormat) / 8;

            var result = Sepia(bitmapData.Scan0, img.Height, img.Width, bytesPerPixel, bitmapData.Stride);
            
            img.UnlockBits(bitmapData);

            return result == 0;
        }
    }
}
