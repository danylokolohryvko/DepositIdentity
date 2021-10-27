using Microsoft.AspNetCore.Identity;

namespace DepositIdentity.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsBlocked { get; set; }
    }
}
