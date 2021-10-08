using AutoMapper;
using DepositIdentity.BLL.DTOs;
using DepositIdentity.BLL.Interfaces;
using DepositIdentity.Models;
using IdentityServer4.Events;
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
        private readonly IMapper mapper;
        private readonly IIdentityServerInteractionService interaction;

        public AccountController(IUserService userService, IMapper mapper, IIdentityServerInteractionService interaction)
        {
            this.userService = userService;
            this.mapper = mapper;
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
            var dto = this.mapper.Map<RegisterDTO>(model);
            bool result = await this.userService.Register(dto);
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
            var dto = this.mapper.Map<LoginDTO>(model);
            bool result = await this.userService.Login(dto);
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
