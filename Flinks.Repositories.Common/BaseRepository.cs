using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Flinks.Repositories.Common
{
    public class BaseRepository
    {
        private readonly HttpClient _httpClient;

        public BaseRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        protected async Task<TResponse> PostAsync<TResponse, TPayload>(string route, TPayload payload)
        {
            var serializedLoginRequest = JsonConvert.SerializeObject(
                payload,
                Formatting.None, 
                new JsonSerializerSettings { 
                    NullValueHandling = NullValueHandling.Ignore
                }
            );
            
            var response = await _httpClient.PostAsync(route, new StringContent(serializedLoginRequest));
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync());
        }
    }
}