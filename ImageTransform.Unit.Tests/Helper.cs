using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ImageTransform.Unit.Tests
{
    public static class Helper
    {
        public static List<Color> GetImagePixels(Bitmap processedBitmap)
        {
            var bitmapData = processedBitmap.LockBits(new Rectangle(0, 0, processedBitmap.Width, processedBitmap.Height), ImageLockMode.ReadWrite, processedBitmap.PixelFormat);
            var result = new List<Color>();
            var bytesPerPixel = Image.GetPixelFormatSize(processedBitmap.PixelFormat) / 8;
            var byteCount = bitmapData.Stride * processedBitmap.Height;
            var pixels = new byte[byteCount];
            var ptrFirstPixel = bitmapData.Scan0;
            Marshal.Copy(ptrFirstPixel, pixels, 0, pixels.Length);
            var heightInPixels = bitmapData.Height;
            var widthInBytes = bitmapData.Width * bytesPerPixel;

            for (var y = 0; y < heightInPixels; y++)
            {
                var currentLine = y * bitmapData.Stride;
                for (var x = 0; x < widthInBytes; x = x + bytesPerPixel)
                {
                    int oldBlue = pixels[currentLine + x];
                    int oldGreen = pixels[currentLine + x + 1];
                    int oldRed = pixels[currentLine + x + 2];

                    result.Add(Color.FromArgb((byte)oldRed, (byte)oldGreen, (byte)oldBlue));
                }
            }
            processedBitmap.UnlockBits(bitmapData);
            
            return result;
        }
    }
}