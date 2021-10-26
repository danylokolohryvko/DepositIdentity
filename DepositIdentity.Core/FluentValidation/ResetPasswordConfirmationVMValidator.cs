using DepositIdentity.Core.Models;
using FluentValidation;

namespace DepositIdentity.Core.FluentValidation
{
    class ResetPasswordConfirmationVMValidator : AbstractValidator<ResetPasswordConfirmationViewModel>
    {
        public ResetPasswordConfirmationVMValidator()
        {
            RuleFor(m => m.PasswordConfirmation).Equal(m => m.PasswordConfirmation);
        }
    }
}
