using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flinks.BusinessLayer.Entities;
using Flinks.BusinessLayer.Options;
using Flinks.Login.Repository;
using Flinks.Login.Repository.Entities;

namespace Flinks.BusinessLayer
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _loginRepository;

        public LoginService(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }

        public async Task<LoginResult> LoginAsync(LoginOptions options)
        {
            var loginRequest = new LoginRequest
            {
                Institution = options.Institution,
                Username = options.Username,
                Password = options.Password,
                Save = true,
                RequestId = options.RequestId
            };

            if (!string.IsNullOrWhiteSpace(options.SecurityChallenge))
            {
                loginRequest.SecurityResponses = new Dictionary<string, string[]>
                {
                    { options.SecurityChallenge, new[] {options.SecurityResponse} }
                };
            }
            
            var login = await _loginRepository.GetLoginAsync(loginRequest);

            return new LoginResult
            {
                RequestId = login.RequestId,
                SecurityChallenge = login.SecurityChallenges.Select(s => s.Prompt).FirstOrDefault()
            };
        }
    }
}