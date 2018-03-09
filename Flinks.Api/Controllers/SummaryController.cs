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
        private readonly IAccountsSummaryService _accountsSummaryService;

        public SummaryController(
            ILoginService loginService,
            IOptions<MockUserOptions> mockUserOptions,
            IAccountsSummaryService accountsSummaryService)
        {
            _loginService = loginService;
            _mockUserOptions = mockUserOptions;
            _accountsSummaryService = accountsSummaryService;
        }
        
        [HttpGet]
        public async Task<AccountsSummary> Get()
        {
            // login logic should normally be extracted in a different action or controller depending on business needs
            var loginResult = await LogInAsync();

            return await _accountsSummaryService.GetAccountsSummaryAsync(loginResult.RequestId);
        }

        private async Task<LoginResult> LogInAsync()
        {
            var loginResult = await GetLoginResultAsync();

            if (string.IsNullOrWhiteSpace(loginResult.SecurityChallenge)) return loginResult;
            
            _mockUserOptions.Value.SecurityResponses.TryGetValue(loginResult.SecurityChallenge,
                out var securityResponse);

            loginResult = await GetLoginResultAsync(
                loginResult.RequestId,
                loginResult.SecurityChallenge,
                securityResponse
            );

            return loginResult;
        }

        private async Task<LoginResult> GetLoginResultAsync(
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