using System;

namespace Flinks.Repositories.AccountsDetail.Entities
{
    public class AccountsDetailRequest
    {
        public Guid RequestId { get; set; }
        public DaysOfTransaction? DaysOfTransactions { get; set; }
    }
}