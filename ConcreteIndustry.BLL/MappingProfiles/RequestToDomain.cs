using AutoMapper;
using ConcreteIndustry.BLL.DTOs.Requests;
using ConcreteIndustry.BLL.DTOs.Responses.UserTokens;
using ConcreteIndustry.DAL.Entities;

namespace ConcreteIndustry.BLL.MappingProfiles
{
    public class RequestToDomain : Profile
    {
        public RequestToDomain()
        {
            CreateMap<CreateConcreteMixRequest, ConcreteMix>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<UpdateConcreteMixRequest, ConcreteMix>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<RegisterUserRequest, AppUser>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<LoginUserRequest, AppUser>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<UpdateAppUserRequest, AppUser>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<CreateProjectRequest, Project>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<UpdateProjectRequest, Project>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<UserTokenDTO, UserToken>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
