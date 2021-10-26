using DepositIdentity.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DepositIdentity.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly IAdminService adminService;

        public AdminController(IAdminService adminService)
        {
            this.adminService = adminService;
        }

        public IActionResult Index(int startIndex = 0, int count = 50)
        {
            var users = this.adminService.GetUsers(startIndex, count);
            return View(users);
        }

        public async Task<IActionResult> Block(string userId)
        {
            await this.adminService.BlockUserAsync(userId);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Unblock(string userId)
        {
            await this.adminService.UnblockUserAsync(userId);
            return RedirectToAction("Index");
        }
    }
}
