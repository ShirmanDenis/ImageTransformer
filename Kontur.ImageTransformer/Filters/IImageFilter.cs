using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Kontur.ImageTransformer.Filters
{
    public interface IImageFilter
    {
        IImageFilterParam[] Params { get; }
        Bitmap Filtrate(Bitmap bitmap);
        IImageFilterParam AddParam();
    }
}