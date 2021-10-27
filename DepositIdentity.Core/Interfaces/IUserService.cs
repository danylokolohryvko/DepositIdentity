using DepositIdentity.Core.Models;
using System.Threading.Tasks;

namespace DepositIdentity.Core.Interfaces
{
    public interface IUserService
    {
        public Task<bool> RegisterAsync(RegisterViewModel model);

        public Task<bool> ConfirmEmailAsync(string username, string token);

        public Task<bool> LoginAsync(LoginViewModel model);

        public Task<bool> ResetPassword(ResetPasswordViewModel model);

        public Task<bool> ResetPasswordConfirmation(ResetPasswordConfirmationViewModel model);

        public Task LogoutAsync();
    }
}
