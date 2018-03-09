using System;
using System.Threading.Tasks;
using Flinks.Repositories.AccountsDetail.Entities;

namespace Flinks.BusinessLayer.Builders
{
    public interface IBiggestCreditTrxIdBuilder
    {
        void ClearAccounts();
        Task AddAccountAsync(Account account);
        Guid Build();
    }
}