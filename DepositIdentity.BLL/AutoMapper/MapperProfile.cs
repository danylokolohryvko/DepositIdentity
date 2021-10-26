using AutoMapper;
using DepositIdentity.Core.Models;

namespace DepositIdentity.BLL.AutoMapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RegisterViewModel, ApplicationUser>();
        }
    }
}
