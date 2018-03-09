using System;
using System.Collections.Generic;

namespace Flinks.BusinessLayer.Entities
{
    public class AccountsSummary
    {
        public Guid LoginId { get; set; }
        public Guid RequestId { get; set; }
        public Holder Holder { get; set; }
        public IEnumerable<OperationAccount> OperationAccounts { get; set; }
        public IEnumerable<UsdAccount> USDAccounts { get; set; }
        public Guid BiggestCreditTrxId { get; set; }
    }
}