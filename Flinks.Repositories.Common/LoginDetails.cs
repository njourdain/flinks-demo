using System;

namespace Flinks.Repositories.Common
{
    public class LoginDetails
    {
        public string Username { get; set; }
        public bool IsScheduledRefresh { get; set; }
        public DateTime LastRefresh { get; set; }
        public Guid Id { get; set; }
    }
}