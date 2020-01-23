using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace Kontur.ImageTransformer.ImageFilters
{
    public interface IImageFilter
    {
        string Name { get; }
        bool Filtrate(Bitmap bitmap, params object[] parameters);
    }
}