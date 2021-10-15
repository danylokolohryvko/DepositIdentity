using AutoMapper;
using DepositIdentity.BLL.Interfaces;
using DepositIdentity.Core.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DepositIdentity.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService userService;
        private readonly IIdentityServerInteractionService interaction;

        public AccountController(IUserService userService, IIdentityServerInteractionService interaction)
        {
            this.userService = userService;
            this.interaction = interaction;
        }

        [HttpGet]
        public IActionResult Register([FromQuery] string returnUrl)
        {
            var model = new RegisterViewModel { ReturnUrl = returnUrl };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            bool result = await this.userService.Register(model);

            if (result && model.ReturnUrl != null)
            {
                return Redirect(model.ReturnUrl);
            }
            else if (result)
            {
                return Ok();
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Login([FromQuery]string returnUrl)
        {
            var model = new LoginViewModel { ReturnUrl = returnUrl };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool result = await this.userService.Login(model);

            if (result && model.ReturnUrl != null)
            {
                return Redirect(model.ReturnUrl);
            }
            else if (result)
            {
                return Ok();
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var context = await interaction.GetLogoutContextAsync(logoutId);
            string returnUrl = context?.PostLogoutRedirectUri;
            await this.userService.Logout();

            if (returnUrl == null)
            {
                return BadRequest();
            }

            return  Redirect(returnUrl);
        }

        [Authorize]
        public IActionResult Test()
        {
            return Ok(User.Identity.Name);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
