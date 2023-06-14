using AutoMapper;
using ItemBookingApp_API.Areas.OrganisationAdmin.Resources.Role;
using ItemBookingApp_API.Domain.Models.Identity;

namespace ItemBookingApp_API.Mappings
{
    public class RoleMappingProfile : Profile
    {
        public RoleMappingProfile()
        {
            CreateMap<Role, RoleResource>();
        }
        
    }
}
