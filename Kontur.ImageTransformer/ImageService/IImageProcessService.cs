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

        Bitmap Process(Bitmap Bitmap, IImageFilter filter, Rectangle scope, ref bool cancel);

        Rectangle ToCropArea(Size imgSize, int x, int y, int w, int h);
    }
}
