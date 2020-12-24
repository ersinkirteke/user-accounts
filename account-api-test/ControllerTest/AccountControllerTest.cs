using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using service_harness.Services;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using user_account_api.Controllers;
using Xunit;

namespace account_api_test.ControllerTest
{
    public class AccountControllerTest
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IEmailService> _emailServiceMock;

        public AccountControllerTest()
        {
            _configurationMock = new Mock<IConfiguration>();
            _emailServiceMock = new Mock<IEmailService>();
        }

        [Fact]
        public void When_Try_To_Reset_Password_Succeed()
        {
            //Arrange
            var mockFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{'AccessToken':'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE2MDg3NjQzMzksImV4cCI6MTYwODc2NDM5OSwiaXNzIjoid3d3LnRlc3QuY29tIiwiYXVkIjoid3d3LnRlc3QyLmNvbSJ9.HHDoG2UjPrGGborrNvxoYIFsxNbmFlK67EMvFt9uKYo','Expiration':'12/24/2020 2:26:31 AM','RefreshToken':'wLb46m3sDX3NGt7T+2z8xe9uw1uDDPrWVrbetN2Xt2U='}"),
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);
            var controller = new AccountController(mockFactory.Object, _configurationMock.Object, _emailServiceMock.Object);
            string email = "ersinkirteke@gmail.com";
            //Act
            var response = controller.ForgetPassword(email);
            //Assert
            Assert.NotNull(response);
            Assert.Equal("Mail sended!", response.Result);
        }

        [Fact]
        public void When_Try_To_Login_Temporarily_With_Valid_Token_Succeed()
        {
            //Arrange
            var mockFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("true"),
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);
            var controller = new AccountController(mockFactory.Object, _configurationMock.Object, _emailServiceMock.Object);
            string email = "ersinkirteke@gmail.com";
            //Act
            var response = controller.TempLogin(email);
            //Assert
            Assert.NotNull(response);
            Assert.Equal("Hello Player,You login successfully", response.Result.Value);
        }
    }
}
