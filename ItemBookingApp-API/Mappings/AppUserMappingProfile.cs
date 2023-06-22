using AutoMapper;
using ItemBookingApp_API.Areas.Resources.AppUser;
using ItemBookingApp_API.Domain.Models.Identity;
using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Extension;
using ItemBookingApp_API.Resources.Order;

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
                  .ForMember(dest => dest.TypeName, opt =>
                  {
                      opt.MapFrom(src => src.AccountType.GetAccountType());
                  })
                 .ForMember(dest => dest.Status, opt => {
                     opt.MapFrom(src => src.Status.ToDescriptionString());
                 });

            CreateMap<SaveUserResource, AppUser>();

            CreateMap<UpdateUserResource, AppUser>();

            CreateMap<UserQueryResource, UserQuery>();

        }
    }
}
