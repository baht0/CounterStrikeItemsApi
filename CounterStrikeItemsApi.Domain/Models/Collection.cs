using CounterStrikeItemsApi.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace CounterStrikeItemsApi.Domain.Models
{
    public class Collection : IReferenceEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Slug { get; set; } = null!;

        [Required]
        public Guid TypeId { get; set; }
        public CollectionType Type { get; set; } = null!;

        public ICollection<ItemCommon> ItemCommons { get; set; } = [];
    }
}
