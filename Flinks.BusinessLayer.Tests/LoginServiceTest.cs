using System.Threading.Tasks;
using Flinks.BusinessLayer.Options;
using Flinks.Repositories.Login;
using Flinks.Repositories.Login.Entities;
using Moq;
using Xunit;

namespace Flinks.BusinessLayer.Tests
{
    public class LoginServiceTest
    {
        [Fact]
        public async Task It_UsesNullSecurityResponses_WhenNoneIsGiven()
        {
            // Arrange
            var options = new LoginOptions();
            
            LoginRequest request = null;
            
            var repository = new Mock<ILoginRepository>();
            repository.Setup(r => r.GetLoginAsync(It.IsAny<LoginRequest>()))
                .Callback<LoginRequest>(l => request = l)
                .ReturnsAsync(new LoginResponse());
            
            var service = new LoginService(repository.Object);
            
            // Act
            await service.LoginAsync(options);
            
            // Assert
            Assert.Null(request.SecurityResponses);
        }
        
        [Fact]
        public async Task It_ReturnsSecurityChallengePrompt_WhenOneIsInResponse()
        {
            // Arrange
            var options = new LoginOptions();

            var securityChallenge = new SecurityChallenge {Prompt = "mock"};
            
            var repository = new Mock<ILoginRepository>();
            repository.Setup(r => r.GetLoginAsync(It.IsAny<LoginRequest>()))
                .ReturnsAsync(new LoginResponse { SecurityChallenges = new []{securityChallenge}});
            
            var service = new LoginService(repository.Object);
            
            // Act
            var result = await service.LoginAsync(options);
            
            // Assert
            Assert.Equal("mock", result.SecurityChallenge);
        }
        
        [Fact]
        public async Task It_ReturnsNullSecurityChallengePrompt_WhenNoneIsInResponse()
        {
            // Arrange
            var options = new LoginOptions();
            
            var repository = new Mock<ILoginRepository>();
            repository.Setup(r => r.GetLoginAsync(It.IsAny<LoginRequest>()))
                .ReturnsAsync(new LoginResponse());
            
            var service = new LoginService(repository.Object);
            
            // Act
            var result = await service.LoginAsync(options);
            
            // Assert
            Assert.Null(result.SecurityChallenge);
        }
    }
}