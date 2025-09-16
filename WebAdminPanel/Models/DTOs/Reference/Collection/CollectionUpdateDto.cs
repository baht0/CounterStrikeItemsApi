using WebAdminPanel.Models.DTOs.Reference;

namespace WebAdminPanel.Models.DTOs.Reference.Collection
{
    public class CollectionUpdateDto : ReferenceUpdateDto, IReferenceUpdateDto
    {
        public Guid TypeId { get; set; }
    }
}
