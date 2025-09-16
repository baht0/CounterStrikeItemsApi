using System.ComponentModel.DataAnnotations;

namespace CounterStrikeItemsApi.Domain.Models
{
    public class ItemCommon
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Slug { get; set; } = null!;

        //Связи

        public Guid TypeId { get; set; }
        public ItemType Type { get; set; } = null!;

        public Guid SubtypeId { get; set; }
        public Subtype Subtype { get; set; } = null!;

        public Guid? CollectionId { get; set; }
        public Collection? Collection { get; set; }

        public Guid? TournamentId { get; set; }
        public Tournament? Tournament { get; set; }

        public Guid? TeamId { get; set; }
        public Team? Team { get; set; }

        public Guid? ProfessionalPlayerId { get; set; }
        public ProfessionalPlayer? ProfessionalPlayer { get; set; }

        public ICollection<Item> Items { get; set; } = [];
        public ICollection<Found> FoundsAsItem { get; set; } = [];
        public ICollection<Found> FoundsAsContainer { get; set; } = [];
    }
}
