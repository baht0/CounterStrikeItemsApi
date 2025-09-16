using System.ComponentModel.DataAnnotations;

namespace CounterStrikeItemsApi.Domain.Models
{
    public class Item
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Slug { get; set; } = null!;
        public long? ItemNameId { get; set; }
        public string? ImageId { get; set; }

        // Связи
        public Guid ItemCommonId { get; set; }
        public ItemCommon ItemCommon { get; set; } = null!;

        public Guid? ExteriorId { get; set; }
        public Exterior? Exterior { get; set; }

        public Guid? CategoryId { get; set; }
        public Category? Category { get; set; }

        public Guid? QualityId { get; set; }
        public Quality? Quality { get; set; }

        public Guid? GraffitiColorId { get; set; }
        public GraffitiColor? GraffitiColor { get; set; }

        public ICollection<ItemOrdersHistory>? ItemOrdersHistories { get; set; } = [];
    }
}
