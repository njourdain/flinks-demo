using System;

namespace Flinks.BusinessLayer.Entities
{
    public class LoginResult
    {
        public Guid RequestId { get; set; }
        public string SecurityChallenge { get; set; }
    }
}