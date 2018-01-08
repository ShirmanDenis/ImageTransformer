using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.ImageTransformer.ImageService
{
    public interface IImageServiceOptions
    {
        /// <summary>
        /// The maximum image size (in bytes) that the service can process
        /// </summary>
        long MaxImageSize { get; set; }
    }
}
