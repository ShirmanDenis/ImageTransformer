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
    public class GrayscaleFilter : IImageFilter
    {
        [DllImport("ImgProc.dll")]
        private static extern int Grayscale(IntPtr imgData, int height, int width, int stride, int bytesPerPixel);

        public IImageFilterParam[] Params { get; } = new IImageFilterParam[0];

        public bool Filtrate(Bitmap img)
        {
            if (img == null) throw new ArgumentNullException(nameof(img));

            var bitmapData = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadWrite, img.PixelFormat);
            var bytesPerPixel = Image.GetPixelFormatSize(img.PixelFormat) / 8;

            var result = Grayscale(bitmapData.Scan0, img.Height, img.Width, bitmapData.Stride, bytesPerPixel);

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
