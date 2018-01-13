using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace Kontur.ImageTransformer.Filters
{
    public interface IImageFilter
    {
        IImageFilterParam[] Params { get; }
        void Filtrate(Bitmap bitmap);
        Task FiltrateAsync(Bitmap bitmap);
        IImageFilterParam AddParam(object value);
    }
}