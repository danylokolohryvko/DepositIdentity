using DepositIdentity.Core.Models;
using System;
using System.Collections.Generic;

namespace DepositIdentity.Core.Interfaces
{
    public interface IUserRepository
    {
        public List<ApplicationUser> GetUsers(int startIndex, int count);
    }
}
