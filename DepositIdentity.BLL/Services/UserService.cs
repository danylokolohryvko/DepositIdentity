using AutoMapper;
using DepositIdentity.BLL.DTOs;
using DepositIdentity.BLL.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DepositIdentity.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IMapper mapper;

        public UserService(UserManager<IdentityUser> userManager, IMapper mapper, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
        }

        public async Task<bool> Register(RegisterDTO model)
        {
            var user = this.mapper.Map<IdentityUser>(model);
            var result = await this.userManager.CreateAsync(user, model.Password);
            if(result.Succeeded)
            {
                user = await this.userManager.FindByNameAsync(model.UserName);
                await this.signInManager.SignInAsync(user,false);
            }
            return result.Succeeded;
        }

        public async Task<bool> Login(LoginDTO model)
        {
            await this.signInManager.SignOutAsync();
            var result = await this.signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
            return result.Succeeded;
        }

        public async Task Logout()
        {
            await this.signInManager.SignOutAsync();
        }
    }
}
