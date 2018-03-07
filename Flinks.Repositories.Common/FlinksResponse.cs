using System;

namespace Flinks.Repositories.Common
{
    public class FlinksResponse
    {
        public int HttpResponseCode { get; set; }
        public LoginDetails Login { get; set; }
        public string Institution { get; set; }
        public Guid RequestId { get; set; }
    }
}