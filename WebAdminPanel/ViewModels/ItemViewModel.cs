using System.ComponentModel.DataAnnotations;

namespace WebAdminPanel.ViewModels
{
    public class ItemViewModel : IEditElement
    {
        public Guid? Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        public string? ImageId { get; set; }
        public Guid? ExteriorId { get; set; }
        public string? ExteriorName { get; set; }
        public Guid? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryHexColor { get; set; }
        public Guid? QualityId { get; set; }
        public string? QualityName { get; set; }
        public string? QualityHexColor { get; set; }
        public Guid? GraffitiColorId { get; set; }
        public string? GraffitiColorName { get; set; }
        public string? GraffitiColorHexColor { get; set; }

        public ItemStatus? Status { get; set; }
    }
}
