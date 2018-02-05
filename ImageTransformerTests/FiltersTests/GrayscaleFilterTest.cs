using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using ImageTransformerTests.Properties;
using Kontur.ImageTransformer.Filters;
using NUnit.Framework;

namespace ImageTransformerTests
{
    [TestFixture]
    public class GrayscaleFilterTest
    {
        private readonly GrayscaleFilter _grayscaleFilter = new GrayscaleFilter();
        private bool _cancel;
        [Test]
        public void SepiaFilter_ThrowArgumentNullException_WhenImageIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _grayscaleFilter.Filtrate(null));
        }

        [Test]
        public void GrayscaleFilterAlgoTest()
        {
            var testImg = Resources.AlphaImg;
            var expectedPixelsArray = Helper.GetImagePixels(testImg).Select(Grayscale);

            _grayscaleFilter.Filtrate(testImg);
            var actualPixelsArray = Helper.GetImagePixels(testImg);

            CollectionAssert.AreEqual(expectedPixelsArray, actualPixelsArray);
        }

        [Test]
        public void GrayscaleFilter_AddParam_shouldReturn_Null()
        {
            var param = _grayscaleFilter.AddParam(null);

            param.Should().BeNull();

            _grayscaleFilter.Params.Length.ShouldBeEquivalentTo(0);
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
