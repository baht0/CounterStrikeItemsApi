using System.ComponentModel.DataAnnotations;

namespace CounterStrikeItemsApi.Application.DTOs.Items
{
    public class ItemCreateDto
    {
        [Required(ErrorMessage = "Item name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name length must be between 3 and 100 characters.")]
        public string Name { get; set; } = null!;

        [StringLength(300, MinimumLength = 50, ErrorMessage = "ImageId must be between 50 and 300 characters.")]
        public string? ImageId { get; set; }
        public int? ItemNameId { get; set; }

        public Guid? ExteriorId { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? QualityId { get; set; }
        public Guid? GraffitiColorId { get; set; }
    }
}
