using DepositIdentity.Core.Interfaces;
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
        public async Task<IActionResult> RegisterAsync(RegisterViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            bool result = await this.userService.RegisterAsync(model);

            if (result)
            {
                return RedirectToAction("ConfirmEmailNotification", "Account", new { });
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult ConfirmEmailNotification()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmailAsync(string username, string token, string returnUrl)
        {
            await this.userService.ConfirmEmailAsync(username, token);

            if(returnUrl != null)
            {
                return Redirect(returnUrl);
            }

            return Ok();
        }

        [HttpGet]
        public IActionResult Login([FromQuery]string returnUrl)
        {
            var model = new LoginViewModel { ReturnUrl = returnUrl };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool result = await this.userService.LoginAsync(model);

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
        public IActionResult ResetPassword(string returnUrl)
        {
            var model = new ResetPasswordViewModel { ReturnUrl = returnUrl };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordViewModel model)
        {
            var result = await this.userService.ResetPassword(model);

            if(result)
            {
                return View("PasswordChangeNotification");
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation(string username, string token, string returnUrl)
        {
            var model = new ResetPasswordConfirmationViewModel
            {
                Username = username,
                Token = token,
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPasswordConfirmationAsync(ResetPasswordConfirmationViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await this.userService.ResetPasswordConfirmation(model);

            if(result)
            {
                return Redirect(model.ReturnUrl);
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> LogoutAsync(string logoutId)
        {
            var context = await interaction.GetLogoutContextAsync(logoutId);
            string returnUrl = context?.PostLogoutRedirectUri;
            await this.userService.LogoutAsync();

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
