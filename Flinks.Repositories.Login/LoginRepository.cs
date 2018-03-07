using System.Net.Http;
using System.Threading.Tasks;
using Flinks.Repositories.Common;
using Flinks.Repositories.Login.Entities;

namespace Flinks.Repositories.Login
{
    public class LoginRepository : BaseRepository, ILoginRepository
    {
        private const string AuthorizeRoute = "BankingServices/Authorize";

        public LoginRepository(HttpClient httpClient) : base(httpClient)
        {
        }
        
        public async Task<LoginResponse> GetLoginAsync(LoginRequest loginRequest)
        {
            return await PostAsync<LoginResponse, LoginRequest>(AuthorizeRoute, loginRequest);
        }
    }
}