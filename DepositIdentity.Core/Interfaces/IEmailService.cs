using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DepositIdentity.Core.Interfaces
{
    public interface IEmailService
    {
        public Task SendAsync(string to, string subject, string body);
    }
}
