using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Kontur.ImageTransformer.ImageFilters;
using Kontur.ImageTransformer.FiltersFactory;
using Moq;
using NUnit.Framework;

namespace ImageTransformerTests
{
    [TestFixture]
    public class FiltersFactoryTests
    {
        private FiltersFactory _factory;
        private Mock<IImageFilter> _mockFilter;

        [SetUp]
        public void Init()
        {
            _factory = new FiltersFactory();
            _mockFilter = new Mock<IImageFilter>();
        }

        [Test]
        public void IsRegistered_ShouldReturn_True_WhenFilterIsRegistered()
        {
            _factory.RegisterFilter("grayscale", _mockFilter.Object);

            _factory.IsRegistered("grayscale").Should().BeTrue();
        }

        [Test]
        public void IsRegistered_ShouldReturn_False_WhenFilterIsRegistered()
        {
            _factory.IsRegistered("grayscale").Should().BeFalse();
        }

        [Test]
        public void GetFilter_ShouldReturn_Filter_WhenFilterIsRegistered()
        {
            _factory.RegisterFilter("grayscale", _mockFilter.Object);

            var filter = _factory.GetFilter("grayscale");

            filter.Should().NotBeNull();
        }

        [Test]
        public void GetFilter_ShouldThrowException_WhenFilterIsNotRegistered()
        {
            _factory.Invoking( f => f.GetFilter("grayscale"))
                .ShouldThrow<Exception>()
                .WithMessage("Filter with name \"grayscale\" is not registered");
        }

        [Test]
        public void DoubleRegisterFilter_Should_ThrowException()
        {
            _factory.RegisterFilter("grayscale", _mockFilter.Object);

            _factory
                .Invoking(f => f.RegisterFilter("grayscale", _mockFilter.Object))
                .ShouldThrow<Exception>()
                .WithMessage("Filter with name \"grayscale\" already registered");
        }
    }
}
