using System;
using FluentAssertions;
using ImageTransform.Api.FiltersFactory;
using ImageTransform.Api.ImageFilters;
using NUnit.Framework;

namespace ImageTransform.Unit.Tests
{
    [TestFixture]
    public class FiltersFactoryTests
    {
        private FiltersFactory _factory;
        private IImageFilter _mockFilter;

        [SetUp]
        public void Init()
        {
            _factory = new FiltersFactory();
            _mockFilter = new SepiaFilter();
        }

        [Test]
        public void IsRegistered_ShouldReturn_True_WhenFilterIsRegistered()
        {
            _factory.RegisterFilter("grayscale", _mockFilter);

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
            _factory.RegisterFilter("grayscale", _mockFilter);

            var filter = _factory.GetFilter("grayscale");

            filter.Should().NotBeNull();
        }

        [Test]
        public void GetFilter_ShouldThrowException_WhenFilterIsNotRegistered()
        {
            _factory.Invoking( f => f.GetFilter("grayscale"))
                .Should()
                .Throw<Exception>()
                .WithMessage("Filter with name \"grayscale\" is not registered");
        }

        [Test]
        public void DoubleRegisterFilter_Should_ThrowException()
        {
            _factory.RegisterFilter("grayscale", _mockFilter);

            _factory
                .Invoking(f => f.RegisterFilter("grayscale", _mockFilter))
                .Should()
                .Throw<Exception>()
                .WithMessage("Filter with name \"grayscale\" already registered");
        }
    }
}
