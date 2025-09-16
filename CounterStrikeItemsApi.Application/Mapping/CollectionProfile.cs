using AutoMapper;
using CounterStrikeItemsApi.Application.DTOs.Reference;
using CounterStrikeItemsApi.Application.DTOs.Reference.Collection;
using CounterStrikeItemsApi.Application.Helpers;
using CounterStrikeItemsApi.Domain.Models;

namespace CounterStrikeItemsApi.Application.Mapping
{
    public class CollectionProfile : Profile
    {
        public CollectionProfile()
        {
            CreateMap<Collection, ReferenceDto>();

            CreateMap<Collection, CollectionDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type));

            CreateMap<CollectionDto, Collection>()
                .ForMember(dest => dest.Type, opt => opt.Ignore());

            CreateMap<CollectionCreateDto, Collection>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => StringToSlug.Generate(src.Name)));

            CreateMap<CollectionUpdateDto, Collection>()
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => StringToSlug.Generate(src.Name)));
        }
    }
}
