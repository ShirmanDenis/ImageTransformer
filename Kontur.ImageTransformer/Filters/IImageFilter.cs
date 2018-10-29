using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace Kontur.ImageTransformer.Filters
{
    public interface IImageFilter
    {
        bool Filtrate(Bitmap bitmap, params object[] parameters);
    }
}