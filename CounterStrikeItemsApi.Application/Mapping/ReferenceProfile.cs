using AutoMapper;
using CounterStrikeItemsApi.Application.DTOs.Reference;
using CounterStrikeItemsApi.Application.Helpers;
using CounterStrikeItemsApi.Domain.Models;

namespace CounterStrikeItemsApi.Application.Mapping
{
    public class ReferenceProfile : Profile
    {
        public ReferenceProfile()
        {
            CreateMap<Category, ReferenceColorDto>();
            CreateMap<CollectionType, ReferenceDto>();
            CreateMap<Exterior, ReferenceDto>();
            CreateMap<GraffitiColor, ReferenceColorDto>();
            CreateMap<ProfessionalPlayer, ReferenceDto>();
            CreateMap<Quality, ReferenceColorDto>();
            CreateMap<Team, ReferenceDto>();
            CreateMap<Tournament, ReferenceDto>();

            CreateMap<ReferenceCreateDto, Category>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => StringToSlug.Generate(src.Name)));
            CreateMap<ReferenceCreateDto, CollectionType>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => StringToSlug.Generate(src.Name)));
            CreateMap<ReferenceCreateDto, Exterior>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => StringToSlug.Generate(src.Name)));
            CreateMap<ReferenceCreateDto, GraffitiColor>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => StringToSlug.Generate(src.Name)));
            CreateMap<ReferenceCreateDto, ProfessionalPlayer>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => StringToSlug.Generate(src.Name)));
            CreateMap<ReferenceCreateDto, Quality>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => StringToSlug.Generate(src.Name)));
            CreateMap<ReferenceCreateDto, Team>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => StringToSlug.Generate(src.Name)));
            CreateMap<ReferenceCreateDto, Tournament>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => StringToSlug.Generate(src.Name)));

            CreateMap<ReferenceUpdateDto, Category>()
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => StringToSlug.Generate(src.Name)))
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<ReferenceUpdateDto, CollectionType>()
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => StringToSlug.Generate(src.Name)))
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<ReferenceUpdateDto, Exterior>()
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => StringToSlug.Generate(src.Name)))
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<ReferenceUpdateDto, GraffitiColor>()
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => StringToSlug.Generate(src.Name)))
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<ReferenceUpdateDto, ProfessionalPlayer>()
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => StringToSlug.Generate(src.Name)))
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<ReferenceUpdateDto, Quality>()
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => StringToSlug.Generate(src.Name)))
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<ReferenceUpdateDto, Team>()
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => StringToSlug.Generate(src.Name)))
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<ReferenceUpdateDto, Tournament>()
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => StringToSlug.Generate(src.Name)))
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
