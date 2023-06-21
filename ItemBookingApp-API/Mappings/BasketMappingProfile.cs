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

            CreateMap<IBasketItem, BasketItem>();

            CreateMap<BasketItem, BasketItemResource>()
                 .ForMember(dest => dest.Name, opt =>
                 {
                     opt.MapFrom(src => src.Item.GetItemName());
                 })
                .ForMember(dest => dest.Price, opt =>
                {
                    opt.MapFrom(src => src.Item.GetItemPrice());
                })
                .ForMember(dest => dest.Type, opt =>
                {
                    opt.MapFrom(src => src.Item.GetItemType());
                })
                .ForMember(dest => dest.Picture, opt =>
                {
                    opt.MapFrom(src => src.Item.GetItemPicture());
                });

            CreateMap<CustomerBasketResource, CustomerBasket>();

            CreateMap<SaveCustomerBasketResource, CustomerBasket>();


            CreateMap<UpdateCustomerBasketResource, CustomerBasket>();
        }
    }
}
