using System.Collections.Generic;

namespace Flinks.Api
{
    public class MockUserOptions
    {
        public string Institution { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public IDictionary<string, string> SecurityResponses { get; set; } = new Dictionary<string, string>();
    }
}