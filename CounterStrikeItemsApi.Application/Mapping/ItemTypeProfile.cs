using AutoMapper;
using CounterStrikeItemsApi.Application.DTOs.Reference;
using CounterStrikeItemsApi.Application.DTOs.Reference.ItemType;
using CounterStrikeItemsApi.Application.Helpers;
using CounterStrikeItemsApi.Domain.Models;

namespace CounterStrikeItemsApi.Application.Mapping
{
    public class ItemTypeProfile : Profile
    {
        public ItemTypeProfile()
        {
            CreateMap<ItemType, ReferenceDto>();

            CreateMap<ItemType, ItemTypeDto>()
                .ForMember(dest => dest.Subtypes,
                    opt => opt.MapFrom(src => src.Subtypes.Select(x => x.Subtype)));

            CreateMap<ItemTypeDto, ItemType>();

            CreateMap<ItemTypeCreateDto, ItemType>()
                 .ForMember(d => d.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                 .ForMember(d => d.Slug, opt => opt.MapFrom(s => StringToSlug.Generate(s.Name)))
                 .ForMember(d => d.Subtypes, opt => opt.Ignore())
                 .AfterMap((src, dest) =>
                 {
                     dest.Subtypes = [.. (src.SubtypeIds ?? [])
                         .Distinct()
                         .Select(subtypeId => new ItemTypeSubtype
                         {
                             ItemTypeId = dest.Id,
                             SubtypeId = subtypeId
                         })];
                 });

            CreateMap<ItemTypeUpdateDto, ItemType>()
                 .ForMember(d => d.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                 .ForMember(d => d.Slug, opt => opt.MapFrom(s => StringToSlug.Generate(s.Name)))
                 .ForMember(d => d.Subtypes, opt => opt.Ignore())
                 .AfterMap((src, dest) =>
                 {
                     dest.Subtypes = [.. (src.SubtypeIds ?? [])
                         .Distinct()
                         .Select(subtypeId => new ItemTypeSubtype
                         {
                             ItemTypeId = dest.Id,
                             SubtypeId = subtypeId
                         })];
                 });
        }
    }
}
