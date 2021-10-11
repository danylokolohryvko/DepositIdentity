using AutoMapper;
using DepositIdentity.BLL.DTOs;
using DepositIdentity.BLL.Interfaces;
using IdentityServer4.Events;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DepositIdentity.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IMapper mapper;
        private readonly IIdentityServerInteractionService interaction;
        private readonly IEventService events;

        public UserService(UserManager<IdentityUser> userManager, IMapper mapper, SignInManager<IdentityUser> signInManager, IIdentityServerInteractionService interaction, IEventService events)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
            this.interaction = interaction;
            this.events = events;
        }

        public async Task<bool> Register(RegisterDTO model)
        {
            var user = this.mapper.Map<IdentityUser>(model);
            var result = await this.userManager.CreateAsync(user, model.Password);

            if(result.Succeeded)
            {
                user = await this.userManager.FindByNameAsync(model.Username);
                await this.signInManager.SignInAsync(user, true);
            }

            return result.Succeeded;
        }

        public async Task<bool> Login(LoginDTO model)
        {
            var context = await this.interaction.GetAuthorizationContextAsync(model.ReturnUrl);
            var user = await this.userManager.FindByNameAsync(model.Username);
            await this.signInManager.SignOutAsync();
            var result = await this.signInManager.PasswordSignInAsync(user, model.Password, true, false);

            if(result.Succeeded)
            {
                await events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName, clientId: context?.Client.ClientId));
            }
            else
            {
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
