using AutoMapper;
using WebAdminPanel.Models.DTOs.Reference;
using WebAdminPanel.Models.DTOs.Reference.Collection;
using WebAdminPanel.Models.DTOs.Reference.ItemType;
using WebAdminPanel.Models.DTOs.Reference.Subtype;
using WebAdminPanel.ViewModels;

namespace WebAdminPanel.Mapping
{
    public class ReferenceProfile : Profile
    {
        public ReferenceProfile()
        {
            CreateMap<ReferenceDto, ReferenceViewModel>();
            CreateMap<ReferenceViewModel, ReferenceDto>();

            // ReferenceViewModel → Guid (для контейнеров)
            CreateMap<ReferenceViewModel, Guid>()
                .ConvertUsing(src => src.Id);

            //Edit & Create
            CreateMap<ReferenceDto, ReferenceUpdateDto>();
            CreateMap<ReferenceUpdateDto, ReferenceCreateDto>();

            CreateMap<ReferenceColorDto, ReferenceColorUpdateDto>();
            CreateMap<ReferenceColorUpdateDto, ReferenceColorCreateDto>();

            //Collection
            CreateMap<CollectionDto, CollectionUpdateDto>();
            CreateMap<CollectionUpdateDto, CollectionCreateDto>();

            //Type
            CreateMap<ItemTypeDto, ItemTypeUpdateDto>()
                .ForMember(dest => dest.Subtypes,
                    opt => opt.MapFrom(src => src.Subtypes.Select(x => x.Id)));
            CreateMap<ItemTypeUpdateDto, ItemTypeCreateDto>()
                .ForMember(dest => dest.Subtypes,
                    opt => opt.MapFrom(src => src.Subtypes));

            //Subtype
            CreateMap<SubtypeDto, SubtypeUpdateDto>()
                .ForMember(dest => dest.ItemTypes,
                    opt => opt.MapFrom(src => src.ItemTypes.Select(x => x.Id)));
            CreateMap<SubtypeUpdateDto, SubtypeCreateDto>()
                .ForMember(dest => dest.ItemTypeIds,
                    opt => opt.MapFrom(src => src.ItemTypes));
        }
    }
}
