using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flinks.Repositories.AccountsDetail.Entities;

namespace Flinks.BusinessLayer.Builders
{
    public class UsdAccountsBuilder : IUsdAccountsBuilder
    {
        private readonly ConcurrentBag<Entities.UsdAccount> _accounts = new ConcurrentBag<Entities.UsdAccount>();

        public void ClearAccounts()
        {
            _accounts.Clear();
        }

        public Task AddAccountAsync(Account account)
        {
            return Task.Run(() =>
            {
                if (account.Currency != Currency.USD) return;
                
                _accounts.Add(new Entities.UsdAccount
                {
                    Balance = account.Balance.Current
                });
            });
        }

        public IEnumerable<Entities.UsdAccount> Build()
        {
            return _accounts;
        }
    }
}