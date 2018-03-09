using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flinks.Repositories.AccountsDetail.Entities;

namespace Flinks.BusinessLayer.Builders
{
    public class OperationAccountsBuilder : IOperationAccountsBuilder
    {
        private readonly ConcurrentBag<Entities.OperationAccount> _accounts = new ConcurrentBag<Entities.OperationAccount>();

        public void ClearAccounts()
        {
            _accounts.Clear();
        }

        public Task AddAccountAsync(Account account)
        {
            return Task.Run(() =>
            {
                if (account.Category != AccountCategory.Operations) return;
                if (!int.TryParse(account.AccountNumber, out var accountNumber)) return;
                _accounts.Add(new Entities.OperationAccount {AccountNumber = accountNumber});
            });
        }

        public IEnumerable<Entities.OperationAccount> Build()
        {
            return _accounts;
        }
    }
}