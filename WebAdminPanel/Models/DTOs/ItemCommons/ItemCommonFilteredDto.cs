using WebAdminPanel.Models.DTOs.Reference.Collection;
using WebAdminPanel.Models.DTOs.Reference.ItemType;
using WebAdminPanel.Models.DTOs.Reference.Subtype;

namespace WebAdminPanel.Models.DTOs.ItemCommons
{
    public class ItemCommonFilteredDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public ItemTypeDto Type { get; set; } = null!;
        public SubtypeDto Subtype { get; set; } = null!;
        public CollectionDto? Collection { get; set; }

        public string? ImageId { get; set; }
        public List<string> Categories { get; set; } = [];
        public List<string> Qualities { get; set; } = [];
        public List<string> Exteriors { get; set; } = [];

        public int FoundIn { get; set; }
        public int Variants { get; set; }
    }
}
