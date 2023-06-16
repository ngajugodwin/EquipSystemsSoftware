using ItemBookingApp_API.Areas.Resources.Item;
using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Domain.Models;
using AutoMapper;
using ItemBookingApp_API.Areas.Resources.ItemType;

namespace ItemBookingApp_API.Mappings
{
    public class ItemTypeMappingProfile : Profile
    {
        public ItemTypeMappingProfile()
        {
            CreateMap<ItemType, ItemTypeResource>()
                .ForMember(dest => dest.Status, opt =>
                {
                    opt.MapFrom(src => src.IsActive ? "Active" : "Inactive");
                });

            CreateMap<ItemTypeResource, ItemType>();

            CreateMap<SaveItemTypeResource, ItemType>();

            CreateMap<UpdateItemTypeResource, ItemType>();

            CreateMap<ItemTypeQueryResource, ItemTypeQuery>();
        }
    }
}
