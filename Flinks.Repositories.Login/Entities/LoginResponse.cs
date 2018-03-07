using Flinks.Repositories.Common;

namespace Flinks.Repositories.Login.Entities
{
    public class LoginResponse : FlinksResponse
    {
        public SecurityChallenge[] SecurityChallenges { get; set; } = new SecurityChallenge[0];
    }
}