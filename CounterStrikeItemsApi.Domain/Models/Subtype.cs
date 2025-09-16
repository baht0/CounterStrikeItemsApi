using CounterStrikeItemsApi.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace CounterStrikeItemsApi.Domain.Models
{
    public class Subtype : IReferenceEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Slug { get; set; } = null!;

        public ICollection<ItemCommon> ItemCommons { get; set; } = [];
        public ICollection<ItemTypeSubtype> ItemTypes { get; set; } = [];
    }
}