using WebAdminPanel.Models.DTOs.Items;

namespace WebAdminPanel.Models.DTOs.ItemCommons
{
    public class ItemCommonCreateBody
    {
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
