using DepositIdentity.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace DepositIdentity.Filters
{
    public class DenyAccessToBlockedUsers : IAsyncActionFilter
    {
        public UserManager<ApplicationUser> userManager;
        public SignInManager<ApplicationUser> signIn;

        public DenyAccessToBlockedUsers(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signIn)
        {
            this.userManager = userManager;
            this.signIn = signIn;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) 
        {
            var user = await this.userManager.GetUserAsync(context.HttpContext.User);
            if (user != null && user.IsBlocked)
            {
                await signIn.SignOutAsync();
                context.ModelState.AddModelError(String.Empty, "You have been blocked");
                context.Result = new RedirectToActionResult("Login", "Account", new { context.ModelState });
            }
            else 
            {
                await next.Invoke();
            }
        }
    }
}
