using AutoMapper;
using CounterStrikeItemsApi.Application.DTOs.Items;
using CounterStrikeItemsApi.Application.Helpers;
using CounterStrikeItemsApi.Domain.Models;

namespace CounterStrikeItemsApi.Application.Mapping
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<Item, ItemDto>();

            CreateMap<ItemCreateDto, Item>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => StringToSlug.Generate(src.Name)))
                .ForMember(dest => dest.ItemCommonId, opt => opt.Ignore()); // будет назначен вручную после маппинга

            CreateMap<ItemUpdateDto, Item>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => StringToSlug.Generate(src.Name)))
                .ForMember(dest => dest.ItemCommonId, opt => opt.Ignore());


            CreateMap<Item, ItemWorkerUpdateDto>();
        }
    }            
}
