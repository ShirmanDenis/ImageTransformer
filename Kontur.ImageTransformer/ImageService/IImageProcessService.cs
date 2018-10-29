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
        ImageServiceOptions Options { get; set; }

        Bitmap Process(Bitmap bitmap, Rectangle scope, IImageFilter filter, params object[] filterParams);

        Rectangle ToCropArea(Size imgSize, int x, int y, int w, int h);
    }
}
