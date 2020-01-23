using System;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using ImageTransform.Api.ImageFilters;
using NUnit.Framework;

namespace ImageTransform.Unit.Tests.FiltersTests
{
    [TestFixture]
    public class SepiaFilterTest
    {
        private readonly SepiaFilter _filter = new SepiaFilter();
        [Test]
        public void SepiaFilter_ThrowArgumentNullException_WhenImageIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _filter.Filtrate(null));
        }

        [Test]
        public void SepiaFilterAlgoTest()
        {
            var testImg = Resources.Resources.AlphaImg;
            var expectedPixelsArray = Helper.GetImagePixels(testImg).Select(Sepia);

            var success = _filter.Filtrate(testImg);
            var actualPixelsArray = Helper.GetImagePixels(testImg);

            success.Should().BeTrue();
            CollectionAssert.AreEqual(expectedPixelsArray, actualPixelsArray);
        }

        private Color Sepia(Color pixel)
        {
            var R = (pixel.R * .393f) + (pixel.G * .769f) + (pixel.B * .189f);
            var G = (pixel.R * .349f) + (pixel.G * .686f) + (pixel.B * .168f);
            var B = (pixel.R * .272f) + (pixel.G * .534f) + (pixel.B * .131f);

            if (R > 255)
                R = 255;
            if (G > 255)
                G = 255;
            if (B > 255)
                B = 255;

            return Color.FromArgb((byte) R, (byte) G, (byte) B);
        }
    }
}