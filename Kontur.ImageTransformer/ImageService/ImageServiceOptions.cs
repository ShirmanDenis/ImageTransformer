namespace Kontur.ImageTransformer.ImageService
{
    public class ImageServiceOptions : IImageServiceOptions
    {
        public long MaxImageSize { get; set; } = 100 * 1024;
    }
}