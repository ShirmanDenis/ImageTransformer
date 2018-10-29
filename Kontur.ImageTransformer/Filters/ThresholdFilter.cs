using System;
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
    public class ThresholdFilter : IImageFilter
    {
        [DllImport("ImgProc.dll")]
        private static extern int Threshold(IntPtr imgData, int value, int height, int width, int bytesPerPixel, int stride);

        public bool Filtrate(Bitmap img, params object[] parameters)
        {
            if (img == null) throw new ArgumentNullException(nameof(img));

            var bitmapData = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadWrite, img.PixelFormat);
            var bytesPerPixel = Image.GetPixelFormatSize(img.PixelFormat) / 8;

            var value = parameters.FirstOrDefault();
            if (value == null)
                throw new ArgumentNullException(nameof(parameters), "Parameter cannot be null");
            
            var result = Threshold(bitmapData.Scan0, (int)value, img.Height, img.Width, bytesPerPixel, bitmapData.Stride);

            img.UnlockBits(bitmapData);

            return result == 0;
        }
    }
}
