﻿using AutoMapper;
using ItemBookingApp_API.Domain.Models.OrderAggregate;
using ItemBookingApp_API.Resources.Order;

namespace ItemBookingApp_API.Mappings
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<AddressDto, Domain.Models.OrderAggregate.Address>();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.ShippingPrice, o => o.MapFrom(s => s.DeliveryMethod.Price));


            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ItemId, o => o.MapFrom(s => s.ItemOrdered.ItemId))
                .ForMember(d => d.ItemName, o => o.MapFrom(s => s.ItemOrdered.Name))
                .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.ItemOrdered.PictureUrl));


        }
    }
}
