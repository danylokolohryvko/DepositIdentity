using AutoMapper;
using DepositIdentity.Core.Interfaces;
using DepositIdentity.Core.Models;
using IdentityServer4.Events;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Threading.Tasks;
using System.Web;

namespace DepositIdentity.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IActionContextAccessor contextAccessor;
        private readonly IMapper mapper;
        private readonly IIdentityServerInteractionService interaction;
        private readonly IEventService events;
        private readonly IEmailService emailService;

        public UserService(UserManager<ApplicationUser> userManager, IMapper mapper, SignInManager<ApplicationUser> signInManager, IActionContextAccessor contextAccessor,
            IIdentityServerInteractionService interaction, IEventService events, IEmailService emailService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.contextAccessor = contextAccessor;
            this.mapper = mapper;
            this.interaction = interaction;
            this.events = events;
            this.emailService = emailService;
        }

        public async Task<bool> RegisterAsync(RegisterViewModel model)
        {
            var user = this.mapper.Map<ApplicationUser>(model);
            var result = await this.userManager.CreateAsync(user, model.Password);
            if(result.Succeeded)
            {
                await SendEmailConfirmTokenAsync(user, model.ReturnUrl);
            }

            return result.Succeeded;
        }

        public async Task<bool> ConfirmEmailAsync(string username, string token)
        {
            var user = await this.userManager.FindByNameAsync(username);
            var result = await this.userManager.ConfirmEmailAsync(user, token);
            if(result.Succeeded)
            {
                await this.signInManager.SignInAsync(user, true);
            }

            return result.Succeeded;
        }

        public async Task<bool> LoginAsync(LoginViewModel model)
        {
            var context = await this.interaction.GetAuthorizationContextAsync(model.ReturnUrl);
            var user = await this.userManager.FindByNameAsync(model.Username);

            if (user == null)
            {
                this.contextAccessor.ActionContext.ModelState.AddModelError(string.Empty, "Incorrect username");
            }
            else if (user.IsBlocked)
            {
                contextAccessor.ActionContext.ModelState.AddModelError(string.Empty, "You have been blocked");

                return false;
            }
            else if(!user.EmailConfirmed)
            {
                contextAccessor.ActionContext.ModelState.AddModelError(string.Empty, "You need confirm email");

                return false;
            }

            await this.signInManager.SignOutAsync();
            var result = await this.signInManager.PasswordSignInAsync(user, model.Password, true, false);

            if(result.Succeeded)
            {
                await events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName, clientId: context?.Client.ClientId));
            }
            else
            {
                contextAccessor.ActionContext.ModelState.AddModelError(string.Empty, "Login or password is incorrect");
                await events.RaiseAsync(new UserLoginFailureEvent(model.Username, "invalid credentials", clientId: context?.Client.ClientId));
            }

            return result.Succeeded;
        }

        public async Task<bool> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await this.userManager.FindByNameAsync(model.Username);

            if(user == null)
            {
                this.contextAccessor.ActionContext.ModelState.AddModelError(string.Empty, "Incorrect username");

                return false;
            }
            else if(user.Email != model.Email)
            {
                this.contextAccessor.ActionContext.ModelState.AddModelError(string.Empty, "Incorrect Email");

                return false;
            }

            var token = HttpUtility.UrlEncode(await this.userManager.GeneratePasswordResetTokenAsync(user));
            var username = HttpUtility.UrlEncode(user.UserName);
            var returnUrl = HttpUtility.UrlEncode(model.ReturnUrl);
            await emailService.SendAsync(user.Email, "Change password confirmation", $"Click to link: https://localhost:44394/Account/ResetPasswordConfirmation?username={username}&token={token}&returnUrl={returnUrl}");

            return true;
        }

        public async Task<bool> ResetPasswordConfirmation(ResetPasswordConfirmationViewModel model)
        {
            var user = await this.userManager.FindByNameAsync(model.Username);
            
            if(user == null)
            {
                this.contextAccessor.ActionContext.ModelState.AddModelError(string.Empty, "Incorrect username");
            }

            var result = await this.userManager.ResetPasswordAsync(user, model.Token, model.Password);
            
            if(result.Succeeded)
            {
                await this.signInManager.SignInAsync(user, true);
            }
            else
            {
                this.contextAccessor.ActionContext.ModelState.AddModelError(string.Empty, "Can not reset password");

                return false;
            }

            return true;
        }

        public async Task LogoutAsync()
        {
            await this.signInManager.SignOutAsync();
        }

        private async Task SendEmailConfirmTokenAsync(ApplicationUser user, string returnUrl)
        {
            var token = HttpUtility.UrlEncode(await this.userManager.GenerateEmailConfirmationTokenAsync(user));
            var username = HttpUtility.UrlEncode(user.UserName);
            returnUrl = HttpUtility.UrlEncode(returnUrl);
            await emailService.SendAsync(user.Email, "Deposit email confirmation", $"Click to link: https://localhost:44394/Account/ConfirmEmail?username={username}&token={token}&returnUrl={returnUrl}");
        }
    }
}
