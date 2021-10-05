﻿using DepositIdentity.BLL.AutoMapper;
using DepositIdentity.BLL.Interfaces;
using DepositIdentity.BLL.Services;
using DepositIdentity.DAL.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DepositIdentity.DI
{
    public class Dependencies
    {
        public static void Inject(IServiceCollection services, string connection)
        {
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(connection));
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>();
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 1;
                options.Password.RequiredUniqueChars = 1;
            });
            services.AddScoped<IUserService, UserService>();
            services.AddAutoMapper(typeof(MapperProfile));
        }
    }
}
