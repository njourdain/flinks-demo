using System.Collections.Generic;
using System.Threading.Tasks;
using Flinks.Api;
using Flinks.Api.Controllers;
using Flinks.BusinessLayer;
using Flinks.BusinessLayer.Entities;
using Flinks.BusinessLayer.Options;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Flink.Api.Tests
{
    public class SummaryControllerTest
    {
        [Fact]
        public async Task It_ShouldCompleteLoginWithSecurityResponses()
        {
            // Arrange
            var loginResult = new LoginResult
            {
                SecurityChallenge = "question"
            };
            
            LoginOptions loginOptions = null, loginOptions2 = null;
            
            var mockLoginService = new Mock<ILoginService>();
            mockLoginService.Setup(m => m.LoginAsync(It.IsAny<LoginOptions>()))
                .ReturnsAsync(loginResult)
                .Callback<LoginOptions>(options =>
                {
                    if (loginOptions == null)
                    {
                        loginOptions = options;
                        return;
                    }

                    loginOptions2 = options;
                });
            
            var mockSummaryService = Mock.Of<IAccountsSummaryService>();
            
            var mockOptions = Mock.Of<IOptions<MockUserOptions>>(m => m.Value == new MockUserOptions
            {
                SecurityResponses = new Dictionary<string, string>
                {
                    {"question", "answer"},
                    {"question2", "answer2"}
                }
            });
            
            var controller = new SummaryController(mockLoginService.Object, mockOptions, mockSummaryService);
            
            // Act
            await controller.Get();
            
            // Assert
            mockLoginService.Verify(m => m.LoginAsync(It.IsAny<LoginOptions>()), Times.Exactly(2));
            Assert.Null(loginOptions.SecurityResponse);
            Assert.Equal("answer", loginOptions2.SecurityResponse);
        }
    }
}