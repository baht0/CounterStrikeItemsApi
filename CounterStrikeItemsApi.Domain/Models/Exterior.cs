using CounterStrikeItemsApi.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace CounterStrikeItemsApi.Domain.Models
{
    public class Exterior : IReferenceEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Slug { get; set; } = null!;

        public ICollection<Item> Items { get; set; } = [];
    }
}
