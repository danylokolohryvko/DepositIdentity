using AutoMapper;
using DepositIdentity.BLL.Interfaces;
using DepositIdentity.Core.Models;
using IdentityServer4.Events;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Threading.Tasks;

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

        public UserService(UserManager<ApplicationUser> userManager, IMapper mapper, SignInManager<ApplicationUser> signInManager, IActionContextAccessor contextAccessor, IIdentityServerInteractionService interaction, IEventService events)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.contextAccessor = contextAccessor;
            this.mapper = mapper;
            this.interaction = interaction;
            this.events = events;
        }

        public async Task<bool> Register(RegisterViewModel model)
        {
            var user = this.mapper.Map<ApplicationUser>(model);
            var result = await this.userManager.CreateAsync(user, model.Password);

            if(result.Succeeded)
            {
                user = await this.userManager.FindByNameAsync(model.Username);
                await this.signInManager.SignInAsync(user, true);
            }

            return result.Succeeded;
        }

        public async Task<bool> Login(LoginViewModel model)
        {
            var context = await this.interaction.GetAuthorizationContextAsync(model.ReturnUrl);
            var user = await this.userManager.FindByNameAsync(model.Username);

            if(user != null && user.IsBlocked)
            {
                contextAccessor.ActionContext.ModelState.AddModelError(string.Empty, "You have been blocked");
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

        public async Task Logout()
        {
            await this.signInManager.SignOutAsync();
        }
    }
}
