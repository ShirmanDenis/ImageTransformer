using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kontur.ImageTransformer.Filters
{
    public class SepiaFilter : IImageFilter
    {
        [DllImport("ImgProc.dll")]
        private static extern int Sepia(IntPtr imgData, int height, int width, int bytesPerPixel, int stride, ref bool cancel);

        public IImageFilterParam[] Params { get; } = new IImageFilterParam[0];

        public bool Filtrate(Bitmap img, ref bool cancel)
        {
            if (img == null) throw new ArgumentNullException(nameof(img));

            var bitmapData = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadWrite, img.PixelFormat);
            var bytesPerPixel = Image.GetPixelFormatSize(img.PixelFormat) / 8;

            var result = Sepia(bitmapData.Scan0, img.Height, img.Width, bytesPerPixel, bitmapData.Stride, ref cancel);
            
            img.UnlockBits(bitmapData);

            return result == 0;
        }

        public IImageFilterParam AddParam(object value)
        {
            // Nothing to do here
            return null;
        }
    }
}
