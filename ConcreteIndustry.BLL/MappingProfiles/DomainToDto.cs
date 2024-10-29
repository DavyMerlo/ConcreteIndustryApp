using AutoMapper;
using ConcreteIndustry.BLL.DTOs.Responses.Account;
using ConcreteIndustry.BLL.DTOs.Responses.Addresses;
using ConcreteIndustry.BLL.DTOs.Responses.Clients;
using ConcreteIndustry.BLL.DTOs.Responses.ConcreteMixes;
using ConcreteIndustry.BLL.DTOs.Responses.Materials;
using ConcreteIndustry.BLL.DTOs.Responses.Orders;
using ConcreteIndustry.BLL.DTOs.Responses.Projects;
using ConcreteIndustry.BLL.DTOs.Responses.RefreshTokens;
using ConcreteIndustry.BLL.DTOs.Responses.Users;
using ConcreteIndustry.BLL.DTOs.Responses.UserTokens;
using ConcreteIndustry.DAL.Entities;

namespace ConcreteIndustry.BLL.MappingProfiles
{
    public class DomainToDTO : Profile
    {
        public DomainToDTO()
        {
            CreateMap<ConcreteMix, ConcreteMixDTO>();

            CreateMap<AppUser, AppUserDTO>();

            CreateMap<AppUser, AuthenticationDTO>();

            CreateMap<Client, ClientDTO>(); 

            CreateMap<Address, AddressDTO>();

            CreateMap<Project, ProjectDTO>();

            CreateMap<Material, MaterialDTO>();

            CreateMap<Order, OrderDTO>();

            CreateMap<UserToken, UserTokenDTO>();

            CreateMap<RefreshToken, RefreshTokenDTO>();
        }
    }
}
