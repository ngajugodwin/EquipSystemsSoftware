﻿using AutoMapper;
using ItemBookingApp_API.Areas.Resources.Item;
using ItemBookingApp_API.Domain.Models.Queries;
using ItemBookingApp_API.Domain.Models;
using ItemBookingApp_API.Extension;
using ItemBookingApp_API.Resources.CustomerQueries;

namespace ItemBookingApp_API.Mappings
{
    public class ItemMappingProfile : Profile
    {
        public ItemMappingProfile()
        {
            CreateMap<Item, ItemResource>()
                .ForMember(dest => dest.Status, opt => {
                    opt.MapFrom(src => (bool)src.IsActive ? "Active" : "Inactive");
                })
                .ForMember(dest => dest.ItemState, opt => {
                    opt.MapFrom(src => src.ItemState.ToDescriptionString());
                });

            CreateMap<ItemResource, Item>();

            CreateMap<SaveItemResource, Item>();

            CreateMap<UpdateItemResource, Item>();

            CreateMap<ChangeItemImageResource, Item>()
                .ForMember(dest => dest.Id, opt =>
                {
                    opt.MapFrom(src => src.ItemId);
                });

            CreateMap<ItemQueryResource, ItemQuery>();

            CreateMap<Item, ItemToReturnDto>()
                 .ForMember(dest => dest.ItemType, opt => {
                     opt.MapFrom(src => src.ItemType.Name);
                 })
                  .ForMember(dest => dest.Category, opt => {
                      opt.MapFrom(src => src.Category.Name);
                  });
        }
    }
}
