namespace Flinks.Repositories.AccountsDetail.Entities
{
    public class Account
    {
        public AccountHolder Holder { get; set; }
        public string AccountNumber { get; set; }
        public AccountCategory Category { get; set; }
        public Currency Currency { get; set; }
        public AccountBalance Balance { get; set; }
        public Transaction[] Transactions { get; set; } = new Transaction[0];
    }
}