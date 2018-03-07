using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Flinks.Repositories.AccountsDetail.Entities;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;

namespace Flinks.Repositories.AccountsDetail.Tests
{
    public class AccountsDetailRepositoryTest
    {
        [Fact]
        public async Task It_ShouldSerializeEnumsAsStrings()
        {
            // Arrange
            var accountResponse = new AccountsDetailResponse();
            
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(accountResponse), Encoding.UTF8, "application/json")
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
            var repository = new AccountsDetailRepository(httpClient);
            var accountRequest = new AccountsDetailRequest {DaysOfTransactions = DaysOfTransaction.Days90};
            
            // Act
            await repository.GetAccountsDetailAsync(accountRequest);

            // Assert
            Assert.Contains("\"DaysOfTransactions\":\"Days90\"", await request.Content.ReadAsStringAsync());
        }
        
        [Fact]
        public async Task It_ShouldCallTheCorrectUri()
        {
            // Arrange
            var accountResponse = new AccountsDetailResponse();
            
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(accountResponse), Encoding.UTF8, "application/json")
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
            var repository = new AccountsDetailRepository(httpClient);
            var accountRequest = new AccountsDetailRequest();
            
            // Act
            await repository.GetAccountsDetailAsync(accountRequest);

            // Assert
            Assert.Equal("http://www.mock.com/BankingServices/GetAccountsDetail", request.RequestUri.AbsoluteUri);
        }
        
        [Fact]
        public async Task It_ShouldParseAndReturnALoginResponseObject()
        {
            // Arrange
            var accountResponse = new AccountsDetailResponse
            {
                Accounts = new [] {new Account()}
            };
            
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(accountResponse), Encoding.UTF8, "application/json")
            };

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns((HttpRequestMessage req, CancellationToken cancellationToken) => Task.FromResult(response));
            
            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://www.mock.com")
            };
            var repository = new AccountsDetailRepository(httpClient);
            var accountsRequest = new AccountsDetailRequest();
            
            // Act
            var result = await repository.GetAccountsDetailAsync(accountsRequest);

            // Assert
            Assert.NotEmpty(result.Accounts);
        }
        
        [Fact]
        public async Task It_ShouldThrowAnHttpRequestExceptionIfResponseIsNotSuccess()
        {
            // Arrange
            var accountsResponse = new AccountsDetailResponse();
            
            var response = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent(JsonConvert.SerializeObject(accountsResponse), Encoding.UTF8, "application/json")
            };

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns((HttpRequestMessage req, CancellationToken cancellationToken) => Task.FromResult(response));
            
            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://www.mock.com")
            };
            var repository = new AccountsDetailRepository(httpClient);
            var accountsRequest = new AccountsDetailRequest();
            
            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(async () => await repository.GetAccountsDetailAsync(accountsRequest));
        }
    }
}