using CounterStrikeItemsApi.Application.DTOs.Items;
using System.ComponentModel.DataAnnotations;

namespace CounterStrikeItemsApi.Application.DTOs.ItemCommons
{
    public class ItemCommonCreateBody
    {
        [Required(ErrorMessage = "ItemCommon name is required.")]
        public string Name { get; set; } = null!;

        public Guid TypeId { get; set; }
        public Guid SubtypeId { get; set; }
        public Guid? CollectionId { get; set; }
        public Guid? TournamentId { get; set; }
        public Guid? TeamId { get; set; }
        public Guid? ProfessionalPlayerId { get; set; }

        public ICollection<ItemCreateDto> Items { get; set; } = [];
        public ICollection<Guid> ContainerIds { get; set; } = [];
    }
}
