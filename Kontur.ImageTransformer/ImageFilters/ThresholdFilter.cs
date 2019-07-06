using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Kontur.ImageTransformer.ImageFilters
{
    public class ThresholdFilter : IImageFilter
    {
        private readonly Logger log = LogManager.GetCurrentClassLogger();
        [DllImport("ImgProc.dll")]
        private static extern int Threshold(IntPtr imgData, int value, int height, int width, int bytesPerPixel, int stride);

        public bool Filtrate(Bitmap img, params object[] parameters)
        {
            if (img == null) throw new ArgumentNullException(nameof(img));

            var bitmapData = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadWrite, img.PixelFormat);
            var bytesPerPixel = Image.GetPixelFormatSize(img.PixelFormat) / 8;

            var value = parameters.FirstOrDefault();
            if (!TryParseParam(value, out var intParam, out var error))
            {
                log.Error(error);
                return false;
            }
            var result = Threshold(bitmapData.Scan0, intParam, img.Height, img.Width, bytesPerPixel, bitmapData.Stride);

            img.UnlockBits(bitmapData);

            return result == 0;
        }

        private bool TryParseParam(object param, out int value, out string error)
        {
            error = string.Empty;
            value = 0;
            switch (param)
            {
                case null:
                    throw new ArgumentNullException(nameof(param), "Parameter cannot be null");
                case int intParam:
                    value = intParam;
                    return true;
                case string strParam:
                    value = int.Parse(strParam);
                    return true;
                default:
                    throw new Exception($"Can't parse param of filter {GetType().Name}");
            }
        }
    }
}
