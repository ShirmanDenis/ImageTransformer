using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using FluentAssertions;
using ImageTransformerTests.Properties;
using Kontur.ImageTransformer.Filters;
using NUnit.Framework;

namespace ImageTransformerTests
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
            var testImg = Resources.AlphaImg;
            var expectedPixelsArray = Helper.GetImagePixels(testImg).Select(Sepia);

            _filter.Filtrate(testImg);
            var actualPixelsArray = Helper.GetImagePixels(testImg);

            CollectionAssert.AreEqual(expectedPixelsArray, actualPixelsArray);
        }

        [Test]
        public void SepiaFilter_AddParam_shouldReturn_Null()
        {
            var param = _filter.AddParam(0);

            param.Should().BeNull();

            _filter.Params.Length.ShouldBeEquivalentTo(0);
        }

        private Color Sepia(Color pixel)
        {
            var R = (pixel.R * .393) + (pixel.G * .769) + (pixel.B * .189);
            var G = (pixel.R * .349) + (pixel.G * .686) + (pixel.B * .168);
            var B = (pixel.R * .272) + (pixel.G * .534) + (pixel.B * .131);

            if (R > 255)
                R = 255;
            if (G > 255)
                G = 255;
            if (B > 255)
                B = 255;

            return Color.FromArgb((int) R, (int) G, (int) B);
        }
    }
}