using System;

namespace Flinks.Repositories.AccountsDetail.Entities
{
    public class Transaction
    {
        public double? Credit { get; set; }
        public Guid Id { get; set; }
    }
}