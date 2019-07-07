using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageTransform.Client
{
    public interface IImageTransformClient
    {
        Task<OperationResult<IEnumerable<string>>> GetRegisteredFilters(TimeSpan? timeout = null);
    }
}