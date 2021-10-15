using DepositIdentity.BLL.Interfaces;
using DepositIdentity.Core.Models;
using DepositIdentity.DAL.Repository;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DepositIdentity.BLL.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepository userRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public AdminService(IUserRepository userRepositoty, UserManager<ApplicationUser> userManager)
        {
            this.userRepository = userRepositoty;
            this.userManager = userManager;
        }

        public List<ApplicationUser> GetUsers(int startIndex, int count)
        {
            return this.userRepository.GetUsers(u => true, u => u.Id, startIndex, count);
        }

        public async Task BlockUserAsync(string userId)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            user.IsBlocked = true;
            await this.userManager.UpdateAsync(user);
        }

        public async Task UnblockUserAsync(string userId)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            user.IsBlocked = false;
            await this.userManager.UpdateAsync(user);
        }
    }
}
