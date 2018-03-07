using Flinks.Login.Repository;

namespace Flinks.BusinessLayer
{
    public class LoginService
    {
        private readonly ILoginRepository _loginRepository;

        public LoginService(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }
    }
}