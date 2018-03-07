using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Flinks.Repositories.AccountsDetail.Entities
{
    public class AccountsDetailRequest
    {
        public Guid RequestId { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public DaysOfTransaction? DaysOfTransactions { get; set; }
    }
}