using System.Threading.Tasks;
using Flinks.Login.Repository.Entities;

namespace Flinks.Login.Repository
{
    public interface ILoginRepository
    {
        Task<LoginResponse> GetLoginAsync(LoginRequest loginRequest);
    }
}