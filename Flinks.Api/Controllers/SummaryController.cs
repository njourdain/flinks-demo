using System;
using System.Threading.Tasks;
using Flinks.BusinessLayer;
using Flinks.BusinessLayer.Entities;
using Flinks.BusinessLayer.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Flinks.Api.Controllers
{
    [Route("api/[controller]")]
    public class SummaryController : Controller
    {
        private readonly ILoginService _loginService;
        private readonly IOptions<MockUserOptions> _mockUserOptions;

        public SummaryController(ILoginService loginService, IOptions<MockUserOptions> mockUserOptions)
        {
            _loginService = loginService;
            _mockUserOptions = mockUserOptions;
        }
        
        [HttpGet]
        public async Task<LoginResult> Get()
        {
            // login logic should normally be extracted in a different action or controller depending on business needs
            var loginResult = await GetLoginResult();

            if (!string.IsNullOrWhiteSpace(loginResult.SecurityChallenge))
            {
                _mockUserOptions.Value.SecurityResponses.TryGetValue(loginResult.SecurityChallenge,
                    out var securityResponse);
                
                loginResult = await GetLoginResult(
                    loginResult.RequestId,
                    loginResult.SecurityChallenge,
                    securityResponse
                    );
            }
            
            return loginResult;
        }

        private async Task<LoginResult> GetLoginResult(
            Guid? requestId = null,
            string securityChallenge = null,
            string securityResponse = null)
        {
            return await _loginService.LoginAsync(new LoginOptions
            {
                Institution = _mockUserOptions.Value.Institution,
                Username = _mockUserOptions.Value.Username,
                Password = _mockUserOptions.Value.Password,
                RequestId = requestId,
                SecurityChallenge = securityChallenge,
                SecurityResponse = securityResponse
            });
        }
    }
}