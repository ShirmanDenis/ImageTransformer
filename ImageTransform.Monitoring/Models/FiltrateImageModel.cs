using System.ComponentModel.DataAnnotations;

namespace ImageTransform.Monitoring.Models
{
    public class FiltrateImageModel
    {
        [Required]
        public string FilterName { get; set; }

        [Required]
        public byte[] ImgData { get; set; }
    }
}