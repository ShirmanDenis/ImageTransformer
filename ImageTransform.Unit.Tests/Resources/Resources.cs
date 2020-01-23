using System.Drawing;
using System.IO;

namespace ImageTransform.Unit.Tests.Resources
{
    public class Resources
    {

        private static readonly Bitmap Alpha = new Bitmap(new FileStream("Resources/AlphaImg.png", FileMode.Open));
        private static readonly Bitmap Big = new Bitmap(new FileStream("Resources/BigImage.png", FileMode.Open));
        private static readonly Bitmap Test = new Bitmap(new FileStream("Resources/TestImg.png", FileMode.Open));
        public static Bitmap AlphaImg => (Bitmap) Alpha.Clone();

        public static Bitmap BigImage => (Bitmap) Big.Clone();

        public static Bitmap TestImg => (Bitmap) Test.Clone();
    }
}