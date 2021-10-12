using AutoMapper;
using DepositIdentity.BLL.DTOs;
using Microsoft.AspNetCore.Identity;

namespace DepositIdentity.BLL.AutoMapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RegisterDTO, IdentityUser>();
        }
    }
}
