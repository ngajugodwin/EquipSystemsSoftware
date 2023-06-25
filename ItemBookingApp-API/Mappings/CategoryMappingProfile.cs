using AutoMapper;
using ItemBookingApp_API.Areas.Resources.Category;
using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Extension;

namespace ItemBookingApp_API.Mappings
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<Category, CategoryResource>()
                .ForMember(dest => dest.Status, opt => {
                    opt.MapFrom(src => src.Status.ToDescriptionString());
                });

            CreateMap<SaveCategoryResource, Category>();

            CreateMap<UpdateCategoryResource, Category>();

            CreateMap<CategoryQueryResource, CategoryQuery>();
        }
    }
}
