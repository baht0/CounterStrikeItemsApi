using WebAdminPanel.Models.DTOs.Items;
using WebAdminPanel.Models.DTOs.Reference;
using WebAdminPanel.Models.DTOs.Reference.Collection;
using WebAdminPanel.Models.DTOs.Reference.ItemType;
using WebAdminPanel.Models.DTOs.Reference.Subtype;

namespace WebAdminPanel.Models.DTOs.ItemCommons
{
    public class ItemCommonDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public ItemTypeDto Type { get; set; } = null!;
        public SubtypeDto Subtype { get; set; } = null!;
        public CollectionDto? Collection { get; set; }
        public ReferenceDto? Tournament { get; set; }
        public ReferenceDto? Team { get; set; }
        public ReferenceDto? ProfessionalPlayer { get; set; }

        public List<ItemDto> Items { get; set; } = [];
        public List<ReferenceDto> Containers { get; set; } = [];
    }
}
