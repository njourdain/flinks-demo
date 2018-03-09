using System;
using System.Threading.Tasks;
using Flinks.BusinessLayer.Builders;
using Flinks.BusinessLayer.Entities;
using Flinks.Repositories.AccountsDetail;
using Flinks.Repositories.AccountsDetail.Entities;

namespace Flinks.BusinessLayer
{
    public class AccountsSummaryService : IAccountsSummaryService
    {
        private readonly IAccountsDetailRepository _accountsDetailRepository;
        private readonly IAccountsSummaryBuilder _accountsSummaryBuilder;

        public AccountsSummaryService(IAccountsDetailRepository accountsDetailRepository,
            IAccountsSummaryBuilder accountsSummaryBuilder)
        {
            _accountsDetailRepository = accountsDetailRepository;
            _accountsSummaryBuilder = accountsSummaryBuilder;
        }

        public async Task<AccountsSummary> GetAccountsSummaryAsync(Guid requestId)
        {
            var accountsDetail = await _accountsDetailRepository.GetAccountsDetailAsync(new AccountsDetailRequest
            {
                DaysOfTransactions = DaysOfTransaction.Days90,
                RequestId = requestId
            });

            return await _accountsSummaryBuilder.UseAccounts(accountsDetail.Accounts)
                .UseLoginId(accountsDetail.Login.Id)
                .UseRequestId(requestId)
                .BuildAsync();
        }
    }
}