using System;
using Vostok.Logging.Abstractions;

namespace ImageTransform.Client
{
    public class ImageTransformClientFactory
    {
        public static IImageTransformClient Create(Uri uri, ILog log)
        {
            return new ImageTransformClient(uri, log);
        }
    }
}