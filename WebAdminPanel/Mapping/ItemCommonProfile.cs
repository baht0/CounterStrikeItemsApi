using AutoMapper;
using WebAdminPanel.Models.DTOs.ItemCommons;
using WebAdminPanel.ViewModels;

namespace WebAdminPanel.Mapping
{
    public class ItemCommonProfile : Profile
    {
        public ItemCommonProfile()
        {
            CreateMap<ItemCommonDto, ItemCommonViewModel>();

            CreateMap<ItemCommonViewModel, ItemCommonCreateBody>()
                .ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.Type.Id))
                .ForMember(dest => dest.SubtypeId, opt => opt.MapFrom(src => src.Subtype.Id))
                .ForMember(dest => dest.CollectionId, opt => opt.MapFrom(src => src.Collection != null ? src.Collection.Id : (Guid?)null))
                .ForMember(dest => dest.TournamentId, opt => opt.MapFrom(src => src.Tournament != null ? src.Tournament.Id : (Guid?)null))
                .ForMember(dest => dest.TeamId, opt => opt.MapFrom(src => src.Team != null ? src.Team.Id : (Guid?)null))
                .ForMember(dest => dest.ProfessionalPlayerId, opt => opt.MapFrom(src => src.ProfessionalPlayer != null ? src.ProfessionalPlayer.Id : (Guid?)null))
                .ForMember(dest => dest.ContainerIds,
                    opt => opt.MapFrom(src => src.Containers
                        .Select(c => c.Id)))
                .ForMember(dest => dest.Items,
                    opt => opt.MapFrom(src => src.Items));

            CreateMap<ItemCommonViewModel, ItemCommonUpdateBody>()
                .ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.Type.Id))
                .ForMember(dest => dest.SubtypeId, opt => opt.MapFrom(src => src.Subtype.Id))
                .ForMember(dest => dest.CollectionId, opt => opt.MapFrom(src => src.Collection != null ? src.Collection.Id : (Guid?)null))
                .ForMember(dest => dest.TournamentId, opt => opt.MapFrom(src => src.Tournament != null ? src.Tournament.Id : (Guid?)null))
                .ForMember(dest => dest.TeamId, opt => opt.MapFrom(src => src.Team != null ? src.Team.Id : (Guid?)null))
                .ForMember(dest => dest.ProfessionalPlayerId, opt => opt.MapFrom(src => src.ProfessionalPlayer != null ? src.ProfessionalPlayer.Id : (Guid?)null))
                .ForMember(dest => dest.ContainerIds,
                    opt => opt.MapFrom(src => src.Containers
                        .Select(c => c.Id)))
                .ForMember(dest => dest.Items,
                    opt => opt.MapFrom(src => src.Items));
        }
    }
}
