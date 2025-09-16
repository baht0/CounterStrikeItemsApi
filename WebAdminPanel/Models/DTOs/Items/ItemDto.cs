using WebAdminPanel.Models.DTOs.Reference;

namespace WebAdminPanel.Models.DTOs.Items
{
    public class ItemDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = null!;
        public string? ImageId { get; set; }
        public ReferenceDto? Exterior { get; set; }
        public ReferenceColorDto? Category { get; set; }
        public ReferenceColorDto? Quality { get; set; }
        public ReferenceColorDto? GraffitiColor { get; set; }
    }
}
