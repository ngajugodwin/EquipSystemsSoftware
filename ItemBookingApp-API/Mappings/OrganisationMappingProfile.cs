using ItemBookingApp_API.Areas.Resources.Item;
using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Domain.Models;
using AutoMapper;
using ItemBookingApp_API.Areas.Resources.Organisation;
using ItemBookingApp_API.Extension;

namespace ItemBookingApp_API.Mappings
{
    public class OrganisationMappingProfile : Profile
    {
        public OrganisationMappingProfile()
        {
            CreateMap<Organisation, OrganisationResource>()
                .ForMember(dest => dest.Status, opt => {
                    opt.MapFrom(src => src.Status.ToDescriptionString());
                });

            CreateMap<OrganisationResource, Organisation>();

            CreateMap<SaveOrganisationResource, Organisation>();

            CreateMap<UpdateOrganisationResource, Organisation>();

            CreateMap<OrganisationQueryResource, OrganisationQuery>();
        }
    }
}
