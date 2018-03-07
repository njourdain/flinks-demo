using System.Net.Http;
using System.Threading.Tasks;
using Flinks.Login.Repository.Entities;
using Newtonsoft.Json;

namespace Flinks.Login.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private readonly HttpClient _httpClient;
        private const string AuthorizeRoute = "BankingServices/Authorize";

        public LoginRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<LoginResponse> GetLoginAsync(LoginRequest loginRequest)
        {
            var serializedLoginRequest = JsonConvert.SerializeObject(
                loginRequest,
                Formatting.None, 
                new JsonSerializerSettings { 
                    NullValueHandling = NullValueHandling.Ignore
                }
            );
            
            var response = await _httpClient.PostAsync(AuthorizeRoute, new StringContent(serializedLoginRequest));
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<LoginResponse>(await response.Content.ReadAsStringAsync());
        }
    }
}