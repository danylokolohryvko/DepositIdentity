using DepositIdentity.BLL.AutoMapper;
using DepositIdentity.BLL.Services;
using DepositIdentity.Core.Interfaces;
using DepositIdentity.Core.Models;
using DepositIdentity.DAL.EntityFramework;
using DepositIdentity.DAL.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Mail;

namespace DepositIdentity.DI
{
    public class Dependencies
    {
        public static void Inject(IServiceCollection services, string connection)
        {
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(connection));

            services.AddScoped<IEmailService, EmailService>();

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 1;
                options.Password.RequiredUniqueChars = 1;
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddAutoMapper(typeof(MapperProfile));
        }
    }
}
