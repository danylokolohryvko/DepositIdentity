using DepositIdentity.Core.Models;
using DepositIdentity.DAL.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DepositIdentity.DAL.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DbSet<ApplicationUser> users;

        public UserRepository(ApplicationContext context)
        {
            this.users = context.Set<ApplicationUser>();
        }

        public List<ApplicationUser> GetUsers<Tkey>(Expression<Func<ApplicationUser,bool>> predicate, Func<ApplicationUser, Tkey> orderBy, int startIndex, int count)
        {
            return this.users.Where(predicate).OrderBy(orderBy).Skip(startIndex).Take(count).ToList();
        }
    }
}
