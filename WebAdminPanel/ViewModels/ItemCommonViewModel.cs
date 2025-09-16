using System.ComponentModel.DataAnnotations;
using WebAdminPanel.Models.DTOs.Reference;
using WebAdminPanel.Models.DTOs.Reference.Collection;
using WebAdminPanel.Models.DTOs.Reference.ItemType;
using WebAdminPanel.Models.DTOs.Reference.Subtype;

namespace WebAdminPanel.ViewModels
{
    public class ItemCommonViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public string Slug { get; set; } = null!;

        [Required]
        public ItemTypeDto Type { get; set; } = new();

        [Required]
        public SubtypeDto Subtype { get; set; } = new();

        public CollectionDto? Collection { get; set; }
        public ReferenceDto? Tournament { get; set; }
        public ReferenceDto? Team { get; set; }
        public ReferenceDto? ProfessionalPlayer { get; set; }

        public List<ItemViewModel> Items { get; set; } = [];
        public List<ReferenceViewModel> Containers { get; set; } = [];
    }
}
