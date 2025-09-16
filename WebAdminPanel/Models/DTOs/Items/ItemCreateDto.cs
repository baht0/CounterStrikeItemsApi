namespace WebAdminPanel.Models.DTOs.Items
{
    public class ItemCreateDto
    {
        public string Name { get; set; } = null!;
        public string? ImageId { get; set; }

        public Guid? ExteriorId { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? QualityId { get; set; }
        public Guid? GraffitiColorId { get; set; }
    }
}
