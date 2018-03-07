using Flinks.Repository.Common;

namespace Flinks.Login.Repository.Entities
{
    public class LoginResponse : FlinksResponse
    {
        public SecurityChallenge[] SecurityChallenges { get; set; } = new SecurityChallenge[0];
    }
}