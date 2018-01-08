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
        private static extern void Threshold(IntPtr imgData, int value, int height, int width, int bytesPerPixel, int stride);

        public IImageFilterParam[] Params { get; } = new IImageFilterParam[1];

        public Bitmap Filtrate(Bitmap img)
        {
            var bitmapData = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadWrite, img.PixelFormat);
            var bytesPerPixel = Image.GetPixelFormatSize(img.PixelFormat) / 8;

            var value = Params.FirstOrDefault()?.Value;
            if (value == null) throw new Exception("!!!!!!");
            
            Threshold(bitmapData.Scan0, (int)value, img.Height, img.Width, bytesPerPixel, bitmapData.Stride);

            return img;
        }

        public IImageFilterParam AddParam()
        {
            if (Params[0] != null)
                return Params[0];

            Params[0] = new ThresholdParam();

            return Params[0];
        }
    }
}
