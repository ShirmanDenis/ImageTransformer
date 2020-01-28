using System;
using Vostok.Clusterclient.Core;
using Vostok.Clusterclient.Core.Topology;
using Vostok.Clusterclient.Transport.Webrequest;
using Vostok.Logging.Abstractions;

namespace ImageTransform.Client
{
    public class ImageTransformClientFactory
    {
        public static IImageTransformClient Create(Uri uri, ILog log)
        {
            return new ImageTransformClient(Configure, log);

            void Configure(IClusterClientConfiguration configuration)
            {
                configuration.ClusterProvider = new FixedClusterProvider(uri);
                configuration.Transport = new WebRequestTransport(log);
            }
        }
    }
}