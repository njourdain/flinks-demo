using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flinks.BusinessLayer.Entities;
using Account = Flinks.Repositories.AccountsDetail.Entities.Account;

namespace Flinks.BusinessLayer.Builders
{
    public class AccountsSummaryBuilder : IAccountsSummaryBuilder
    {
        private readonly IOperationAccountsBuilder _operationAccountsBuilder;
        private readonly IUsdAccountsBuilder _usdAccountsBuilder;
        private readonly IBiggestCreditTrxIdBuilder _biggestCreditTrxIdBuilder;
        private IEnumerable<Account> _accounts;
        private Guid _loginId, _requestId;

        public AccountsSummaryBuilder(
            IOperationAccountsBuilder operationAccountsBuilder,
            IUsdAccountsBuilder usdAccountsBuilder,
            IBiggestCreditTrxIdBuilder biggestCreditTrxIdBuilder)
        {
            _operationAccountsBuilder = operationAccountsBuilder;
            _usdAccountsBuilder = usdAccountsBuilder;
            _biggestCreditTrxIdBuilder = biggestCreditTrxIdBuilder;
        }

        public IAccountsSummaryBuilder UseAccounts(IEnumerable<Account> accounts)
        {
            _accounts = accounts;
            return this;
        }

        public IAccountsSummaryBuilder UseLoginId(Guid id)
        {
            _loginId = id;
            return this;
        }

        public IAccountsSummaryBuilder UseRequestId(Guid id)
        {
            _requestId = id;
            return this;
        }

        public async Task<AccountsSummary> BuildAsync()
        {
            var accountsSummary = new AccountsSummary
            {
                LoginId = _loginId,
                RequestId = _requestId
            };
            
            if (_accounts == null || !_accounts.Any())
            {
                return accountsSummary;
            }
            
            ClearInnerBuilderAccounts();

            var builderTasks = AddAccountsToInnerBuilders();

            accountsSummary.Holder = new Holder
            {
                Name = _accounts.FirstOrDefault()?.Holder?.Name,
                Email = _accounts.FirstOrDefault()?.Holder?.Email
            };

            await Task.WhenAll(builderTasks);

            accountsSummary.OperationAccounts = _operationAccountsBuilder.Build();
            accountsSummary.USDAccounts = _usdAccountsBuilder.Build();
            accountsSummary.BiggestCreditTrxId = _biggestCreditTrxIdBuilder.Build();

            return accountsSummary;
        }

        private IEnumerable<Task> AddAccountsToInnerBuilders()
        {
            return _accounts.SelectMany(account => new[]
            {
                _operationAccountsBuilder.AddAccountAsync(account),
                _usdAccountsBuilder.AddAccountAsync(account),
                _biggestCreditTrxIdBuilder.AddAccountAsync(account)
            });
        }

        private void ClearInnerBuilderAccounts()
        {
            _operationAccountsBuilder.ClearAccounts();
            _usdAccountsBuilder.ClearAccounts();
            _biggestCreditTrxIdBuilder.ClearAccounts();
        }
    }
}