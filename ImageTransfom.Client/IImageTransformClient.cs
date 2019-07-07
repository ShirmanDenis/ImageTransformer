using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace ImageTransform.Client
{
    public interface IImageTransformClient
    {
        Task<OperationResult<IEnumerable<string>>> GetRegisteredFilters(TimeSpan? timeout = null);
        Task<OperationResult<byte[]>> FiltrateImage(byte[] imageBytes, string filter, Rectangle rectangle, params object[] @params);
    }
}