using manager_properties_usa.Controllers;
using manager_properties_usa.Models.Dto;
using manager_properties_usa.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace manager_properties_usa_test.ControllerTest
{
    public class AuthenticationControllerTest
    {
        private AuthenticationController _currentController;
        private readonly Mock<IUAuthentication> _bus = new();

        public AuthenticationControllerTest()
        {
            _currentController = new AuthenticationController(
                _bus.Object);
        }

        [TestCase(1, 200)]
        [TestCase(2, 400)]
        public async Task CreateProductTest(int index, int expected)
        {
            //Arrange
            switch (index)
            {
                case 1:
                    _bus
                        .Setup(t => t.Authenticate(It.IsAny<string>()))
                        .Returns(Task.FromResult(It.IsAny<TokenDataDto>()));
                    break;
                case 2:
                    _bus
                        .Setup(t => t.Authenticate(It.IsAny<string>()))
                        .Throws(new Exception("Error"));
                    break;
            }
            //Act
            IActionResult response = await _currentController.AuthenticateUser(It.IsAny<string>());
            var result = response as ObjectResult;

            //Assert
            Assert.That(result is not null ? result.StatusCode ?? 0 : 0, Is.EqualTo(expected));
        }

    }
}
