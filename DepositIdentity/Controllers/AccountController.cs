using AutoMapper;
using DepositIdentity.BLL.DTOs;
using DepositIdentity.BLL.Interfaces;
using DepositIdentity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DepositIdentity.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public AccountController(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var dto = this.mapper.Map<RegisterDTO>(model);
            bool result = await this.userService.Register(dto);
            if(result)
            {
                return Ok("Register!");
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var dto = this.mapper.Map<LoginDTO>(model);
            bool result = await this.userService.Login(dto);
            if(result)
            {
                return Ok("Login!");
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await this.userService.Logout();
            return Ok("Logout!");
        }

        [Authorize]
        public IActionResult Test()
        {
            return Ok($"Welcome {HttpContext.User.Identity.Name}");
        }
    }
}
