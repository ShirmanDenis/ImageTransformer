using System.Drawing;
using ImageTransform.Core.ImageFilters;
using ImageTransform.Core.Services;
using NUnit.Framework;

namespace ImageTransform.Unit.Tests
{
    [TestFixture]
    public class ImageProcessServiceTests
    {
        private readonly ImageProcessService _service = new ImageProcessService();
        private readonly SepiaFilter _filter = new SepiaFilter();

        private static object[] CropAreaCases = {
            new object[] {  0,   0, 100,  100, new Rectangle( 0, 0, 100, 100) },
            new object[] {  1,   1,  99,   99, new Rectangle( 1, 1,  99, 99 ) },
            new object[] { -5,  -5,  10,   10, new Rectangle( 0, 0,   5, 5  ) },
            new object[] { -5, -30, 200,   50, new Rectangle( 0, 0, 100, 20 ) },
            new object[] { 50,  80, -20, -150, new Rectangle(30, 0,  20, 80 ) },
            new object[] { 50,  80, -20, 100,  new Rectangle(30, 80, 20, 20 ) },
            new object[] { 50,  80,  20, -100, new Rectangle(50, 0,  20, 80 ) },
            new object[] {-50,  80, -20, -150, Rectangle.Empty },
            new object[] {101, 101,  10,   10, Rectangle.Empty },
            new object[] {  0, 100, 100,    0, Rectangle.Empty },
            new object[] {100,   0,   0,  100, Rectangle.Empty },
            new object[] { -5,  -5,   0,    0, Rectangle.Empty },
        };

        [Test, TestCaseSource(nameof(CropAreaCases))]        
        public void CropAreaTests(int x, int y, int w, int h, Rectangle expected)
        {
            var image = Resources.Resources.TestImg;
            var cropArea = _service.ToCropArea(image.Size, x, y, w, h);
            
            Assert.AreEqual(expected, cropArea);
            if (cropArea == Rectangle.Empty) return;

            var cropImg = _service.Process(image, cropArea, _filter);
            Assert.AreEqual(expected.Size, cropImg.Size);
        }
    }
}
