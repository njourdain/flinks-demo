using System.Collections.Generic;
using System.Threading.Tasks;
using Flinks.Repositories.AccountsDetail.Entities;

namespace Flinks.BusinessLayer.Builders
{
    public interface IUsdAccountsBuilder
    {
        void ClearAccounts();
        Task AddAccountAsync(Account account);
        IEnumerable<Entities.UsdAccount> Build();
    }
}