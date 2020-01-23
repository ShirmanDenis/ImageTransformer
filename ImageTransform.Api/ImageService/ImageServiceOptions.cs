namespace ImageTransform.Api.ImageService
{
    public class ImageServiceOptions
    {
        /// <summary>
        /// The maximum image size (in bytes) that the service can process
        /// </summary>
        public long MaxImageSize { get; set; } = 100 * 1024;
    }
}