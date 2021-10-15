using DepositIdentity.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DepositIdentity.DAL.Repository
{
    public interface IUserRepository
    {
        public List<ApplicationUser> GetUsers<Tkey>(Expression<Func<ApplicationUser, bool>> predicate, Func<ApplicationUser, Tkey> orderBy, int startIndex, int count);
    }
}
