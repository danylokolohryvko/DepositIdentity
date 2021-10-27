using DepositIdentity.Core.Interfaces;
using DepositIdentity.Core.Models;
using DepositIdentity.DAL.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DepositIdentity.DAL.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DbSet<ApplicationUser> users;

        public UserRepository(ApplicationContext context)
        {
            this.users = context.Set<ApplicationUser>();
        }

        public List<ApplicationUser> GetUsers(int startIndex, int count)
        {
            return this.users.OrderBy(u => u.Id).Skip(startIndex).Take(count).ToList();
        }
    }
}
