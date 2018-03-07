using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Flinks.Login.Repository.Entities;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;

namespace Flinks.Login.Repository.Tests
{
    public class LoginRepositoryTest
    {
        [Fact]
        public async Task It_ShouldNotSendSecurityResponses_WhenThePropertyIsNull()
        {
            // Arrange
            var loginResponse = new LoginResponse();
            
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(loginResponse), Encoding.UTF8, "application/json")
            };

            HttpRequestMessage request = null;
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Callback<HttpRequestMessage, CancellationToken>((req, cancellationToken) => request = req)
                .Returns((HttpRequestMessage req, CancellationToken cancellationToken) => Task.FromResult(response));
            
            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://www.mock.com")
            };
            var repository = new LoginRepository(httpClient);
            var loginRequest = new LoginRequest();
            
            // Act
            await repository.GetLoginAsync(loginRequest);
            var requestContent = await request.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal("{\"Save\":false,\"MostRecentCached\":false}", requestContent);
        }
        
        [Fact]
        public async Task It_ShouldCallTheCorrectUri()
        {
            // Arrange
            var loginResponse = new LoginResponse();
            
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(loginResponse), Encoding.UTF8, "application/json")
            };

            HttpRequestMessage request = null;
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Callback<HttpRequestMessage, CancellationToken>((req, cancellationToken) => request = req)
                .Returns((HttpRequestMessage req, CancellationToken cancellationToken) => Task.FromResult(response));
            
            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://www.mock.com")
            };
            var repository = new LoginRepository(httpClient);
            var loginRequest = new LoginRequest();
            
            // Act
            await repository.GetLoginAsync(loginRequest);

            // Assert
            Assert.Equal("http://www.mock.com/BankingServices/Authorize", request.RequestUri.AbsoluteUri);
        }
        
        [Fact]
        public async Task It_ShouldParseAndReturnALoginResponseObject()
        {
            // Arrange
            var loginResponse = new LoginResponse
            {
                RequestId = Guid.NewGuid()
            };
            
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(loginResponse), Encoding.UTF8, "application/json")
            };

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns((HttpRequestMessage req, CancellationToken cancellationToken) => Task.FromResult(response));
            
            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://www.mock.com")
            };
            var repository = new LoginRepository(httpClient);
            var loginRequest = new LoginRequest();
            
            // Act
            var login = await repository.GetLoginAsync(loginRequest);

            // Assert
            Assert.Equal(login.RequestId, loginResponse.RequestId);
        }
        
        [Fact]
        public async Task It_ShouldThrowAnHttpRequestExceptionIfResponseIsNotSuccess()
        {
            // Arrange
            var loginResponse = new LoginResponse();
            
            var response = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent(JsonConvert.SerializeObject(loginResponse), Encoding.UTF8, "application/json")
            };

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns((HttpRequestMessage req, CancellationToken cancellationToken) => Task.FromResult(response));
            
            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://www.mock.com")
            };
            var repository = new LoginRepository(httpClient);
            var loginRequest = new LoginRequest();
            
            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(async () => await repository.GetLoginAsync(loginRequest));
        }
    }
}