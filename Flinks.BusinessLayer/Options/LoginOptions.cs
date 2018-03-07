using System;

namespace Flinks.BusinessLayer.Options
{
    public class LoginOptions
    {
        public string Institution { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string SecurityChallenge { get; set; }
        public string SecurityResponse { get; set; }
        public Guid? RequestId { get; set; }
    }
}