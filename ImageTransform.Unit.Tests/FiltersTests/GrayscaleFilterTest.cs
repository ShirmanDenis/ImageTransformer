using System;
using System.Drawing;
using System.Linq;
using ImageTransform.Core.ImageFilters;
using NUnit.Framework;

namespace ImageTransform.Unit.Tests.FiltersTests
{
    [TestFixture]
    public class GrayscaleFilterTest
    {
        private readonly GrayscaleFilter _grayscaleFilter = new GrayscaleFilter();
        [Test]
        public void SepiaFilter_ThrowArgumentNullException_WhenImageIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _grayscaleFilter.Filtrate(null));
        }

        [Test]
        public void GrayscaleFilterAlgoTest()
        {
            var testImg = Resources.Resources.AlphaImg;
            var expectedPixelsArray = Helper.GetImagePixels(testImg).Select(Grayscale);

            _grayscaleFilter.Filtrate(testImg);
            var actualPixelsArray = Helper.GetImagePixels(testImg);

            CollectionAssert.AreEqual(expectedPixelsArray, actualPixelsArray);
        }
        
        private Color Grayscale(Color color)
        {
            var intensity = (color.R + color.G + color.B) / 3;
            var R = intensity;
            var G = intensity;
            var B = intensity;
            
            return Color.FromArgb(R, G, B);
        }
    }
}
