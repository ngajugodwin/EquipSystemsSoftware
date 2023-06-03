using AutoMapper;
using ItemBookingApp_API.Areas.Resources.Category;
using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Domain.Models.Queries;

namespace ItemBookingApp_API.Mappings
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<Category, CategoryResource>()
                .ForMember(dest => dest.Status, opt => {
                    opt.MapFrom(src => src.IsActive ? "Active" : "Disabled");
                }); ;

            CreateMap<SaveCategoryResource, Category>();

            CreateMap<UpdateCategoryResource, Category>();

            CreateMap<CategoryQueryResource, CategoryQuery>();
        }
    }
}
