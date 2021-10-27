using DepositIdentity.Core.Models;
using FluentValidation;

namespace DepositIdentity.Core.FluentValidation
{
    public class RegisterVMValidation : AbstractValidator<RegisterViewModel>
    {
        public RegisterVMValidation()
        {
            RuleFor(m => m.Email).EmailAddress();
            RuleFor(m => m.Username).NotEmpty();
            RuleFor(m => m.Password).NotEmpty();
            RuleFor(m => m.PasswordConfirmation).Equal(m => m.PasswordConfirmation);
        }
    }
}
