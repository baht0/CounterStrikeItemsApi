using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CounterStrikeItemsApi.Domain.Models
{
    public class ItemTypeSubtype
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Guid ItemTypeId { get; set; }
        public ItemType ItemType { get; set; } = null!;
        public Guid SubtypeId { get; set; }
        public Subtype Subtype { get; set; } = null!;
    }
}