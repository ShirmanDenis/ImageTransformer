using System.Drawing;
using ImageTransform.Client.Models;

namespace ImageTransform.Monitoring.Models
{
    public class FiltrateModel
    {
        public string FilterName { get; set; }

        public byte[] ImgData { get; set; }

        public Area Area { get; set; }

        public object[] Params { get; set; }

        public FiltrateRequest ToFiltrateRequest()
        {
            return new FiltrateRequest()
            {
                ImgData = ImgData,
                Area = new Rectangle(Area.X, Area.Y, Area.Width, Area.Height),
                Params = Params,
                FilterName = FilterName
            };
        }
    }
}