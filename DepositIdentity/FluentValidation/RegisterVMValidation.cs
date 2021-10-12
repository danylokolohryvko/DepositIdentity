using DepositIdentity.Models;
using FluentValidation;

namespace DepositIdentity.FluentValidation
{
    public class RegisterVMValidation : AbstractValidator<RegisterViewModel>
    {
        public RegisterVMValidation()
        {
            RuleFor(m => m.Username).NotEmpty();
            RuleFor(m => m.Password).NotEmpty();
            RuleFor(m => m.PasswordConfirmation).Equal(m => m.PasswordConfirmation);
        }
    }
}
