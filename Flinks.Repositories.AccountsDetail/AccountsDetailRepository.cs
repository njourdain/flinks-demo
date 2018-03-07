using System.Net.Http;
using System.Threading.Tasks;
using Flinks.Repositories.AccountsDetail.Entities;
using Flinks.Repositories.Common;

namespace Flinks.Repositories.AccountsDetail
{
    public class AccountsDetailRepository : BaseRepository, IAccountsDetailRepository
    {
        private const string AccountsDetailRoute = "BankingServices/GetAccountsDetail";

        public AccountsDetailRepository(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<AccountsDetailResponse> GetAccountsDetailAsync(AccountsDetailRequest request)
        {
            return await PostAsync<AccountsDetailResponse, AccountsDetailRequest>(AccountsDetailRoute, request);
        }
    }
}