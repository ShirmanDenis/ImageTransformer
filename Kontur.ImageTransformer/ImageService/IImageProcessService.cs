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
    public interface IImageProcessService
    {
        IImageServiceOptions ServiceOptions { get; set; }

        Bitmap Process(Bitmap Bitmap, IImageFilter filter, Rectangle scope);
        Task<Bitmap> ProcessAsync(Bitmap Bitmap, IImageFilter filter, Rectangle scope);

        Rectangle ToCropArea(Bitmap bitmap, int x, int y, int w, int h);
    }
}
