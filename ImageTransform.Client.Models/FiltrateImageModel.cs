using System.Drawing;

namespace ImageTransform.Client.Models
{
    public class FiltrateImageModel
    {
        public string FilterName { get; set; }

        public byte[] ImgData { get; set; }

        public Rectangle Area { get; set; }

        public object[] Params { get; set; }
    }
}