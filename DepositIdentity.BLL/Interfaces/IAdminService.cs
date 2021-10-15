using DepositIdentity.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DepositIdentity.BLL.Interfaces
{
    public interface IAdminService
    {
        public List<ApplicationUser> GetUsers(int startIndex, int count);

        public Task BlockUserAsync(string userId);

        public Task UnblockUserAsync(string userId);
    }
}
