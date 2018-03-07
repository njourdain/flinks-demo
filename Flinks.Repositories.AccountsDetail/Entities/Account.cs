namespace Flinks.Repositories.AccountsDetail.Entities
{
    public class Account
    {
        public Holder Holder { get; set; }
        public string AccountNumber { get; set; }
        
        //[JsonConverter(typeof(StringEnumConverter))]
        public AccountCategory Category { get; set; }
        
        //[JsonConverter(typeof(StringEnumConverter))]
        public Currency Currency { get; set; }
        
        public AccountBalance Balance { get; set; }
        
        public Transaction[] Transactions { get; set; } = new Transaction[0];
    }
}