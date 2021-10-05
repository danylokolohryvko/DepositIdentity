using DepositIdentity.BLL.DTOs;
using System.Threading.Tasks;

namespace DepositIdentity.BLL.Interfaces
{
    public interface IUserService
    {
        public Task<bool> Register(RegisterDTO model);

        public Task<bool> Login(LoginDTO model);

        public Task Logout();
    }
}
