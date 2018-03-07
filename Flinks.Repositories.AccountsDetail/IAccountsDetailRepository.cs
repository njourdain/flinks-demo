using System.Threading.Tasks;
using Flinks.Repositories.AccountsDetail.Entities;

namespace Flinks.Repositories.AccountsDetail
{
    public interface IAccountsDetailRepository
    {
        Task<AccountsDetailResponse> GetAccountsDetailAsync(AccountsDetailRequest request);
    }
}