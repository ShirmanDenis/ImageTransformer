using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImageTransform.Client.Models;
using JetBrains.Annotations;

namespace ImageTransform.Client
{
    public interface IImageTransformClient
    {
        [NotNull]
        Task<OperationResult<IEnumerable<string>>> GetFiltersAsync(TimeSpan? timeout = null);
        
        [NotNull]
        Task<OperationResult<byte[]>> FiltrateImageAsync([NotNull] FiltrateRequest requestModel);
    }
}