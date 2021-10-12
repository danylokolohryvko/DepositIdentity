using AutoMapper;
using DepositIdentity.BLL.DTOs;
using DepositIdentity.Models;

namespace DepositIdentity.AutoMapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<LoginViewModel, LoginDTO>();
            CreateMap<RegisterViewModel, RegisterDTO>();
        }
    }
}
