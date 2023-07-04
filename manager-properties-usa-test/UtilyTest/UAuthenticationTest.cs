using manager_properties_usa.Models.Dto;
using manager_properties_usa.Utilities;

namespace manager_properties_usa_test.UtilyTest
{
    public class UAuthenticationTest
    {
        private UAuthentication _currentController;
        private readonly Mock<IUConfiguration> _bus = new();

        public UAuthenticationTest()
        {
            _currentController = new UAuthentication(
                _bus.Object);
        }

        [TestCase(1, "abc123")]
        [TestCase(2, "Error")]
        public async Task CreateProductTest(int index, string expected)
        {
            //Arrange
            switch (index)
            {
                case 1:
                    _bus
                        .Setup(t => t.GenerateToken(It.IsAny<string>()))
                        .Returns("abc123");
                    break;
                case 2:
                    _bus
                        .Setup(t => t.GenerateToken(It.IsAny<string>()))
                        .Throws(new Exception("Error"));
                    break;
            }

            //Act
            TokenDataDto response = new();
            try
            {
                response = await _currentController.Authenticate(It.IsAny<string>());
            }
            catch (Exception ex)
            {
                response.Token = ex.Message;
            }

            //Assert
            Assert.That(response is not null ? response.Token ?? string.Empty : string.Empty, Is.EqualTo(expected));
        }
    }
}
