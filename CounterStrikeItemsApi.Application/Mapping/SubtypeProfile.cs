using AutoMapper;
using CounterStrikeItemsApi.Application.DTOs.Reference;
using CounterStrikeItemsApi.Application.DTOs.Reference.Subtype;
using CounterStrikeItemsApi.Application.Helpers;
using CounterStrikeItemsApi.Domain.Models;

namespace CounterStrikeItemsApi.Application.Mapping
{
    public class SubtypeProfile : Profile
    {
        public SubtypeProfile()
        {
            CreateMap<Subtype, ReferenceDto>();

            CreateMap<Subtype, SubtypeDto>()
                .ForMember(dest => dest.ItemTypes,
                    opt => opt.MapFrom(src => src.ItemTypes.Select(x => x.ItemType)));

            CreateMap<SubtypeDto, Subtype>();

            CreateMap<SubtypeCreateDto, Subtype>()
                 .ForMember(d => d.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                 .ForMember(d => d.Slug, opt => opt.MapFrom(s => StringToSlug.Generate(s.Name)))
                 .ForMember(d => d.ItemTypes, opt => opt.Ignore())
                 .AfterMap((src, dest) =>
                 {
                     dest.ItemTypes = [.. (src.ItemTypeIds ?? [])
                         .Distinct()
                         .Select(typeId => new ItemTypeSubtype
                         {
                             ItemTypeId = typeId,
                             SubtypeId = dest.Id
                         })];
                 });
            
            CreateMap<SubtypeUpdateDto, Subtype>()
                 .ForMember(d => d.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                 .ForMember(d => d.Slug, opt => opt.MapFrom(s => StringToSlug.Generate(s.Name)))
                 .ForMember(d => d.ItemTypes, opt => opt.Ignore())
                 .AfterMap((src, dest) =>
                 {
                     dest.ItemTypes = [.. (src.ItemTypeIds ?? [])
                         .Distinct()
                         .Select(typeId => new ItemTypeSubtype
                         {
                             ItemTypeId = typeId,
                             SubtypeId = dest.Id
                         })];
                 });
        }
    }
}
