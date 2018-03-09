using System;
using System.Linq;
using System.Threading.Tasks;
using Flinks.Repositories.AccountsDetail.Entities;

namespace Flinks.BusinessLayer.Builders
{
    public class BiggestCreditTrxIdBuilder : IBiggestCreditTrxIdBuilder
    {
        private Transaction _currentBiggestCreditTransaction = new Transaction();
        private readonly object _lock = new object();

        public void ClearAccounts()
        {
            _currentBiggestCreditTransaction = new Transaction();
        }

        public Task AddAccountAsync(Account account)
        {
            if (!account.Transactions.Any())
            {
                return Task.CompletedTask;
            }
            
            return Task.Run(() =>
            {
                var biggestTransaction = account.Transactions.Aggregate(
                    (i1, i2) => i1.Credit.GetValueOrDefault(0) > i2.Credit.GetValueOrDefault(0) ? i1 : i2);
                UpdateTransactionIfNecessary(biggestTransaction);
            });
        }

        public Guid Build()
        {
            return _currentBiggestCreditTransaction.Id;
        }

        private void UpdateTransactionIfNecessary(Transaction transaction)
        {
            lock (_lock)
            {
                if (transaction.Credit.GetValueOrDefault(0) > _currentBiggestCreditTransaction.Credit.GetValueOrDefault(0))
                {
                    _currentBiggestCreditTransaction = transaction;
                }
            }
        }
    }
}