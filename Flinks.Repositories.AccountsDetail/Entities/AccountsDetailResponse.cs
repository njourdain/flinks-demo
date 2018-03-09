using Flinks.Repositories.Common;

namespace Flinks.Repositories.AccountsDetail.Entities
{
    public class AccountsDetailResponse : FlinksResponse
    {
        public Account[] Accounts { get; set; } = new Account[0];
    }
}