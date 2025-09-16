using AutoMapper;
using CounterStrikeItemsApi.Application.DTOs.ItemCommons;
using CounterStrikeItemsApi.Application.DTOs.Reference;
using CounterStrikeItemsApi.Application.Helpers;
using CounterStrikeItemsApi.Domain.Models;

namespace CounterStrikeItemsApi.Application.Mapping
{
    public class ItemCommonProfile : Profile
    {
        public ItemCommonProfile()
        {
            CreateMap<ItemCommon, ReferenceDto>();

            CreateMap<ItemCommon, ItemCommonDto>()
                .ForMember(dest => dest.Containers, 
                    opt => opt.MapFrom(src => src.FoundsAsItem.Select(found => found.Container)));

            //Filter
            CreateMap<ItemCommon, ItemCommonFilteredDto>()
                .ForMember(dest => dest.ImageId,
                    opt => opt.MapFrom(src =>
                        src.Items
                            .Select(p => p.ImageId)
                            .FirstOrDefault()))
                .ForMember(dest => dest.Categories,
                    opt => opt.MapFrom(src =>
                        src.Items
                            .Where(i => i.Category != null)
                            .Select(i => i.Category!.Name)
                            .Distinct()
                            .ToList()))
                .ForMember(dest => dest.Qualities,
                    opt => opt.MapFrom(src =>
                        src.Items
                            .Where(i => i.Quality != null)
                            .Select(i => i.Quality!.Name)
                            .Distinct()
                            .ToList()))
                .ForMember(dest => dest.Exteriors,
                    opt => opt.MapFrom(src =>
                        src.Items
                            .Where(i => i.Exterior != null)
                            .Select(i => i.Exterior!.Name)
                            .Distinct()
                            .ToList()))
                .ForMember(dest => dest.FoundIn,
                    opt => opt.MapFrom(f => f.FoundsAsItem.Count))
                .ForMember(dest => dest.Variants,
                    opt => opt.MapFrom(i => i.Items.Count));

            //add
            CreateMap<ItemCommonCreateBody, ItemCommon>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => StringToSlug.Generate(src.Name)))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                .ForMember(dest => dest.FoundsAsItem, opt => opt.MapFrom(src =>
                    src.ContainerIds.Select(itemId => new Found
                    {
                        ItemCommonId = Guid.Empty,
                        ContainerId = itemId
                    })));

            //update
            CreateMap<ItemCommonUpdateBody, ItemCommon>()
                .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => StringToSlug.Generate(src.Name)))
                .ForMember(dest => dest.Items, opt => opt.Ignore())
                .ForMember(dest => dest.FoundsAsItem, opt => opt.Ignore());
        }
    }
}
