using AutoMapper;
using ConcreteIndustry.BLL.DTOs.Responses.RefreshTokens;
using ConcreteIndustry.BLL.DTOs.Responses.Users;
using ConcreteIndustry.BLL.DTOs.Responses.UserTokens;
using ConcreteIndustry.DAL.Entities;

namespace ConcreteIndustry.BLL.MappingProfiles
{
    public class DtoToDomain : Profile
    {
        public DtoToDomain()
        {
            CreateMap<AppUserDTO, AppUser>();

            CreateMap<UserTokenDTO, UserToken>();

            CreateMap<RefreshTokenDTO, RefreshToken>();
        }
    }
}
