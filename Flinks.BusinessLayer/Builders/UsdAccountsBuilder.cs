using System;
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
                
                // Specs say we want the balance as an int, if it's not really the case we could just return double
                var roundedBalance = Math.Round(account.Balance.Current);
                _accounts.Add(new Entities.UsdAccount
                {
                    Balance = roundedBalance > int.MaxValue ? int.MaxValue : (int)roundedBalance
                });
            });
        }

        public IEnumerable<Entities.UsdAccount> Build()
        {
            return _accounts;
        }
    }
}