using System;
using System.Collections.Generic;

namespace Flinks.Login.Repository.Entities
{
    public class LoginRequest
    {
        public string Institution { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Save { get; set; }
        public bool MostRecentCached { get; set; }
        public IDictionary<string, string[]> SecurityResponses { get; set; }
        public Guid? RequestId { get; set; }
    }
}