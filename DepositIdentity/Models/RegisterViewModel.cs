﻿namespace DepositIdentity.Models
{
    public class RegisterViewModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string PasswordConfirmation { get; set; }

        public string ReturnUrl { get; set; }
    }
}
