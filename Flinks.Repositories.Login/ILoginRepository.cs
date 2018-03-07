using System.Threading.Tasks;
using Flinks.Repositories.Login.Entities;

namespace Flinks.Repositories.Login
{
    public interface ILoginRepository
    {
        Task<LoginResponse> GetLoginAsync(LoginRequest loginRequest);
    }
}