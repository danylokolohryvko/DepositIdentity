using AutoMapper;
using DepositIdentity.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace DepositIdentity.BLL.AutoMapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RegisterViewModel, IdentityUser>();
        }
    }
}
