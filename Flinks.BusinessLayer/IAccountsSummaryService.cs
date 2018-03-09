using System;
using System.Threading.Tasks;
using Flinks.BusinessLayer.Entities;

namespace Flinks.BusinessLayer
{
    public interface IAccountsSummaryService
    {
        Task<AccountsSummary> GetAccountsSummaryAsync(Guid requestId);
    }
}