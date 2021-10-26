namespace DepositIdentity.Core.Models
{
    public class ResetPasswordConfirmationViewModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string PasswordConfirmation { get; set; }

        public string ReturnUrl { get; set; }

        public string Token { get; set; }
    }
}
