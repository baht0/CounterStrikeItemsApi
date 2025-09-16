using WebAdminPanel.Models.DTOs.Reference;

namespace WebAdminPanel.Models.DTOs.Reference.Collection
{
    public class CollectionCreateDto : ReferenceCreateDto
    {
        public Guid TypeId { get; set; }
    }
}
