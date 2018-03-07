using System.Threading.Tasks;
using Flinks.BusinessLayer.Entities;
using Flinks.BusinessLayer.Options;

namespace Flinks.BusinessLayer
{
    public interface ILoginService
    {
        Task<LoginResult> LoginAsync(LoginOptions options);
    }
}