using AutoMapper;
using ItemBookingApp_API.Areas.Resources.AppUser;
using ItemBookingApp_API.Domain.Models.Identity;
using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Extension;

namespace ItemBookingApp_API.Mappings
{
    public class AppUserMappingProfile : Profile
    {
        public AppUserMappingProfile()
        {
            //CreateMap<Src, Dest>();

            CreateMap<AppUser, UserResource>()
                 .ForMember(dest => dest.UserRoles, opt =>
                 {
                     opt.MapFrom(src => src.UserRoles.Select(c => c.Role.Name));
                 })
                 .ForMember(dest => dest.OrganisationResource, opt =>
                 {
                     opt.MapFrom(src => src.Organisation.GetOrganisation());
                 })
                 .ForMember(dest => dest.Status, opt => {
                     opt.MapFrom(src => src.IsActive ? "Active" : "Inactive");
                 });

            CreateMap<SaveUserResource, AppUser>();

            CreateMap<UpdateUserResource, AppUser>();

            CreateMap<UsersQueryResource, UserQuery>();
        }
    }
}
