using DepositIdentity.Core.Interfaces;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace DepositIdentity.BLL.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendAsync(string to, string subject, string body)
        {
            MailMessage m = new MailMessage(
                new MailAddress("deposittest12@gmial.com", "Deposit"),
                new MailAddress(to))
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("deposittest12@gmail.com", "1111qqqq////"),
                EnableSsl = true
            };

            await smtp.SendMailAsync(m);
        }
    }
}
