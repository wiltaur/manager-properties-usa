using AutoFixture;
using manager_properties_usa.Data;
using manager_properties_usa.Models.Context;
using manager_properties_usa.Models.Dto;
using manager_properties_usa.Models.Model;
using manager_properties_usa_test.Helpper;
using manager_properties_usa_test.MockDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using System.Reflection.Metadata;

namespace manager_properties_usa_test.DataTest
{
    public class PropertyDataTest : RealEstatePropertyContextMock
    {
        private Fixture _autodata;
        private PropertyData _currentData;
        private readonly Mock<IConfiguration> _configuration = new();
        private readonly Mock<RealEstatePropertyContext> _context;
        public PropertyDataTest()
        {
            _autodata = new Fixture();
            _autodata.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _autodata.Behaviors.Remove(b));
            _autodata.Behaviors.Add(new OmitOnRecursionBehavior(1));
            _context = GetDbContext();
            _currentData = new PropertyData(_context.Object, _configuration.Object);
        }

        [TestCase(1, true)]
        [TestCase(2, true)]
        [TestCase(3, false)]
        public async Task AddPropertyBuildingTest(int index, bool expected)
        {
            //Arrange
            var property = _autodata.Create<PropertyAddDto>();
            property.Images.Clear();

            var mockSetProperty = new Mock<DbSet<Property>>();
            _context.Setup(t => t.Properties).Returns(mockSetProperty.Object);
            var mockSetPropertyImage = new Mock<DbSet<PropertyImage>>();
            _context.Setup(t => t.PropertyImages).Returns(mockSetPropertyImage.Object);

            switch (index)
            {
                case 1:
                    _context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
                    break;
                case 2:
                    var propertyImage = _autodata.Create<PropertyImageDto>();
                    propertyImage.File = "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";

                    property.Images.Add(propertyImage);
                    _context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
                    break;
                case 3:
                    _context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);
                    break;
            }

            ////Act
            bool response = await _currentData.AddPropertyBuilding(property);

            //Assert
            Assert.That(response, Is.EqualTo(expected));
        }

        [TestCase(1, true)]
        [TestCase(2, false)]
        public async Task AddImageFromPropertyTest(int index, bool expected)
        {
            //Arrange
            var property = _autodata.Create<PropertyImagesIdDto>();
            property.Images.Clear();

            var propertyImage = _autodata.Create<PropertyImageDto>();
            propertyImage.File = "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";

            property.Images.Add(propertyImage);

            var mockSetPropertyImage = new Mock<DbSet<PropertyImage>>();
            _context.Setup(t => t.PropertyImages).Returns(mockSetPropertyImage.Object);

            switch (index)
            {
                case 1:
                    _context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
                    break;
                case 2:
                    _context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);
                    break;
            }

            ////Act
            bool response = await _currentData.AddImageFromProperty(property);

            //Assert
            Assert.That(response, Is.EqualTo(expected));
        }

        [TestCase(1, true)]
        [TestCase(2, false)]
        public async Task UpdatePropertyPriceTest(int index, bool expected)
        {
            //Arrange
            var properties = _autodata.Create<List<Property>>().AsQueryable();
            var propertyRequest = _autodata.Create<PropertyPriceDto>();

            properties.First().IdProperty = 1;

            var mockSet = GetMockDbSet(properties);
            _context.Setup(t => t.Properties).Returns(mockSet.Object);

            switch (index)
            {
                case 1:
                    propertyRequest.Id = 1;
                    _context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
                    break;
                case 2:
                    propertyRequest.Id = 2;
                    _context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);
                    break;
            }

            ////Act
            bool response = await _currentData.UpdatePropertyPrice(propertyRequest);

            //Assert
            Assert.That(response, Is.EqualTo(expected));
        }

        [TestCase(1, true)]
        [TestCase(2, false)]
        public async Task UpdatePropertyTest(int index, bool expected)
        {
            //Arrange
            var properties = _autodata.Create<List<Property>>().AsQueryable();
            var propertyRequest = _autodata.Create<PropertyModifyDto>();

            properties.First().IdProperty = 1;

            var mockSet = GetMockDbSet(properties);
            _context.Setup(t => t.Properties).Returns(mockSet.Object);

            switch (index)
            {
                case 1:
                    propertyRequest.Id = 1;
                    _context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
                    break;
                case 2:
                    propertyRequest.Id = 2;
                    _context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);
                    break;
            }

            ////Act
            bool response = await _currentData.UpdateProperty(propertyRequest);

            //Assert
            Assert.That(response, Is.EqualTo(expected));
        }

        [TestCase(1, 1)]
        [TestCase(2, 1)]
        public async Task GetPropertiesTest(int index, int expected)
        {
            //Arrange
            var properties = _autodata.Create<List<Property>>().AsQueryable();
            var propertyRequest = _autodata.Create<PropertyDetailRequestDto>();

            properties.First().Name = "test";
            properties.First().IdOwner = 1;
            properties.First().IdOwnerNavigation.IdOwner = 1;

            var mockSet = GetMockDbSet(properties);
            _context.Setup(t => t.Properties).Returns(mockSet.Object);

            switch (index)
            {
                case 1:
                    propertyRequest.SearchString = "te";
                    _context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
                    break;
                case 2:
                    propertyRequest.SearchString = "";
                    propertyRequest.SortOrderDesc = true;
                    _context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
                    break;
            }
            ////Act
            var response = await _currentData.GetProperties(propertyRequest);

            //Assert
            _context.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
            Assert.That(response.TotalRecords, Is.GreaterThanOrEqualTo(expected));

        }
    }
}
