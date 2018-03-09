using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flinks.BusinessLayer.Entities;
using Flinks.Repositories.AccountsDetail.Entities;

namespace Flinks.BusinessLayer.Builders
{
    public interface IAccountsSummaryBuilder
    {
        IAccountsSummaryBuilder UseAccounts(IEnumerable<Account> accounts);
        IAccountsSummaryBuilder UseLoginId(Guid id);
        IAccountsSummaryBuilder UseRequestId(Guid id);
        Task<AccountsSummary> BuildAsync();
    }
}