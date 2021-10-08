using DepositIdentity.Models;
using FluentValidation;

namespace DepositIdentity.FluentValidation
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
