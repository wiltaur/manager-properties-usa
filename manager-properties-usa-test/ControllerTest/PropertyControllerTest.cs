using manager_properties_usa.Controllers;
using manager_properties_usa.Data.interfaces;
using manager_properties_usa.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace manager_properties_usa_test.ControllerTest
{
    public class PropertyControllerTest
    {
        private PropertyController _currentController;
        private readonly Mock<IPropertyData> _bus = new();

        public PropertyControllerTest()
        {
            _currentController = new PropertyController(
                _bus.Object);
        }

        [TestCase(1, 200)]
        [TestCase(2, 400)]
        [TestCase(3, 400)]
        public async Task AddPropertyBuildingTest(int index, int expected)
        {
            //Arrange
            switch (index)
            {
                case 1:
                    _bus
                        .Setup(t => t.AddPropertyBuilding(It.IsAny<PropertyAddDto>()))
                        .Returns(Task.FromResult(true));
                    break;
                case 2:
                    _bus
                        .Setup(t => t.AddPropertyBuilding(It.IsAny<PropertyAddDto>()))
                        .Returns(Task.FromResult(false));
                    break;
                case 3:
                    _bus
                        .Setup(t => t.AddPropertyBuilding(It.IsAny<PropertyAddDto>()))
                        .Throws(new Exception("Error"));
                    break;
            }
            //Act
            IActionResult response = await _currentController.AddPropertyBuilding(It.IsAny<PropertyAddDto>());
            var result = response as ObjectResult;

            //Assert
            Assert.That(result is not null ? result.StatusCode ?? 0 : 0, Is.EqualTo(expected));
        }

        [TestCase(1, 200)]
        [TestCase(2, 400)]
        [TestCase(3, 400)]
        public async Task AddImageFromPropertyTest(int index, int expected)
        {
            //Arrange
            switch (index)
            {
                case 1:
                    _bus
                        .Setup(t => t.AddImageFromProperty(It.IsAny<PropertyImagesIdDto>()))
                        .Returns(Task.FromResult(true));
                    break;
                case 2:
                    _bus
                        .Setup(t => t.AddImageFromProperty(It.IsAny<PropertyImagesIdDto>()))
                        .Returns(Task.FromResult(false));
                    break;
                case 3:
                    _bus
                        .Setup(t => t.AddImageFromProperty(It.IsAny<PropertyImagesIdDto>()))
                        .Throws(new Exception("Error"));
                    break;
            }
            //Act
            IActionResult response = await _currentController.AddImageFromProperty(It.IsAny<PropertyImagesIdDto>());
            var result = response as ObjectResult;

            //Assert
            Assert.That(result is not null ? result.StatusCode ?? 0 : 0, Is.EqualTo(expected));
        }

        [TestCase(1, 200)]
        [TestCase(2, 400)]
        [TestCase(3, 400)]
        public async Task UpdatePropertyPriceTest(int index, int expected)
        {
            //Arrange
            switch (index)
            {
                case 1:
                    _bus
                        .Setup(t => t.UpdatePropertyPrice(It.IsAny<PropertyPriceDto>()))
                        .Returns(Task.FromResult(true));
                    break;
                case 2:
                    _bus
                        .Setup(t => t.UpdatePropertyPrice(It.IsAny<PropertyPriceDto>()))
                        .Returns(Task.FromResult(false));
                    break;
                case 3:
                    _bus
                        .Setup(t => t.UpdatePropertyPrice(It.IsAny<PropertyPriceDto>()))
                        .Throws(new Exception("Error"));
                    break;
            }
            //Act
            IActionResult response = await _currentController.UpdatePropertyPrice(It.IsAny<PropertyPriceDto>());
            var result = response as ObjectResult;

            //Assert
            Assert.That(result is not null ? result.StatusCode ?? 0 : 0, Is.EqualTo(expected));
        }

        [TestCase(1, 200)]
        [TestCase(2, 400)]
        [TestCase(3, 400)]
        public async Task UpdatePropertyTest(int index, int expected)
        {
            //Arrange
            switch (index)
            {
                case 1:
                    _bus
                        .Setup(t => t.UpdateProperty(It.IsAny<PropertyModifyDto>()))
                        .Returns(Task.FromResult(true));
                    break;
                case 2:
                    _bus
                        .Setup(t => t.UpdateProperty(It.IsAny<PropertyModifyDto>()))
                        .Returns(Task.FromResult(false));
                    break;
                case 3:
                    _bus
                        .Setup(t => t.UpdateProperty(It.IsAny<PropertyModifyDto>()))
                        .Throws(new Exception("Error"));
                    break;
            }
            //Act
            IActionResult response = await _currentController.UpdateProperty(It.IsAny<PropertyModifyDto>());
            var result = response as ObjectResult;

            //Assert
            Assert.That(result is not null ? result.StatusCode ?? 0 : 0, Is.EqualTo(expected));
        }

        [TestCase(1, 200)]
        [TestCase(2, 400)]
        public async Task GetPropertiesTest(int index, int expected)
        {
            //Arrange
            switch (index)
            {
                case 1:
                    _bus
                        .Setup(t => t.GetProperties(It.IsAny<PropertyDetailRequestDto>()))
                        .Returns(Task.FromResult(It.IsAny<PropertyDetailResponseDto>()));
                    break;
                case 2:
                    _bus
                        .Setup(t => t.GetProperties(It.IsAny<PropertyDetailRequestDto>()))
                        .Throws(new Exception("Error"));
                    break;
            }
            //Act
            IActionResult response = await _currentController.GetProperties(It.IsAny<PropertyDetailRequestDto>());
            var result = response as ObjectResult;

            //Assert
            Assert.That(result is not null ? result.StatusCode ?? 0 : 0, Is.EqualTo(expected));
        }
    }
}