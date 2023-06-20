using ItemBookingApp_API.Areas.Resources.Category;
using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Resources.Basket;
using AutoMapper;
using ItemBookingApp_API.Extension;

namespace ItemBookingApp_API.Mappings
{
    public class BasketMappingProfile : Profile
    {
        public BasketMappingProfile()
        {
            CreateMap<CustomerBasket, CustomerBasketResource>();

            CreateMap<BasketItemResource, BasketItem>();

            CreateMap<BasketItem, BasketItemResource>()
                 .ForMember(dest => dest.Item, opt => {
                     opt.MapFrom(src => src.Item.GetItemName());
                 });

            CreateMap<CustomerBasketResource, CustomerBasket>();

            CreateMap<SaveCustomerBasketResource, CustomerBasket>();

            CreateMap<UpdateCustomerBasketResource, CustomerBasket>();
        }
    }
}
