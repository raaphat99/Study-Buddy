using AutoMapper;
using Domain.Models;
using Infrastructure.Identity;

namespace Presentation.Mappings
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            // Map the ApplicationUser to the User to ensure the Domain Layer remains independent of the Infrastructure Layer
            CreateMap<ApplicationUser, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            // Map back from User to ApplicationUser
            CreateMap<User, ApplicationUser>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
        }
    }
}
