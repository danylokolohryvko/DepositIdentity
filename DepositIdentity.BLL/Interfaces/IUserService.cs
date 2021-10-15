using DepositIdentity.Core.Models;
using System.Threading.Tasks;

namespace DepositIdentity.BLL.Interfaces
{
    public interface IUserService
    {
        public Task<bool> Register(RegisterViewModel model);

        public Task<bool> Login(LoginViewModel model);

        public Task Logout();
    }
}
