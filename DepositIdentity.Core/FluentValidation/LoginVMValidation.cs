using DepositIdentity.Core.Models;
using FluentValidation;

namespace DepositIdentity.Core.FluentValidation
{
    public class LoginVMValidation : AbstractValidator<LoginViewModel>
    {
        public LoginVMValidation()
        {
            RuleFor(m => m.Username).NotEmpty();
            RuleFor(m => m.Password).NotEmpty();
        }
    }
}
