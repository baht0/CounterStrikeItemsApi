using AutoMapper;
using WebAdminPanel.Models.DTOs.Items;
using WebAdminPanel.ViewModels;

namespace WebAdminPanel.Mapping
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<ItemDto, ItemViewModel>();

            // ItemViewModel → ItemCreateDto
            CreateMap<ItemViewModel, ItemCreateDto>();

            // ItemViewModel → ItemUpdateDto
            CreateMap<ItemViewModel, ItemUpdateDto>();
        }
    }
}
