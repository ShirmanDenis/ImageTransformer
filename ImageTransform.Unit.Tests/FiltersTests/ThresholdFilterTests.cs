using System;
using System.Drawing;
using System.Linq;
using ImageTransform.Api.ImageFilters;
using NUnit.Framework;
using Vostok.Logging.Console;

namespace ImageTransform.Unit.Tests.FiltersTests
{
    [TestFixture]
    public class ThresholdFilterTests
    {
        private readonly ThresholdFilter _thresholdFilter = new ThresholdFilter(new ConsoleLog());

        [Test]
        public void ThresholdFilter_ThrowArgumentNullException_WhenImageIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _thresholdFilter.Filtrate(null));
        }

        [Test]
        public void ThresholdFilter_ShouldThrowException_whenParameterNotAdded()
        {
            Assert.Throws<ArgumentNullException>(() => _thresholdFilter.Filtrate(Resources.Resources.AlphaImg), "Parameter cannot be null");
        }

        [Test]
        [TestCase(0)]
        [TestCase(10)]
        [TestCase(100)]
        public void ThresholdFilterAlgoTest(int value)
        {
            var testImg = Resources.Resources.AlphaImg;
            var expectedPixelsArray = Helper.GetImagePixels(testImg).Select(c => Threshold(c, value));

            _thresholdFilter.Filtrate(testImg, value);
            var actualPixelsArray = Helper.GetImagePixels(testImg);

            CollectionAssert.AreEqual(expectedPixelsArray, actualPixelsArray);
        }

        private Color Threshold(Color pixel, int x)
        {
            var intensity = (pixel.R + pixel.G + pixel.B) / 3;
            int R, G, B;
            if (intensity >= 255 * x / 100)
            {
                R = 255;
                G = 255;
                B = 255;
            }
            else
            {
                R = 0;
                G = 0;
                B = 0;
            }

            return Color.FromArgb(R, G, B);

        }
    }
}