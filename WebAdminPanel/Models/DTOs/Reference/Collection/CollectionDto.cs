using WebAdminPanel.Models.DTOs.Reference;

namespace WebAdminPanel.Models.DTOs.Reference.Collection
{
    public class CollectionDto : ReferenceDto
    {
        public ReferenceDto Type { get; set; } = null!;
    }
}
