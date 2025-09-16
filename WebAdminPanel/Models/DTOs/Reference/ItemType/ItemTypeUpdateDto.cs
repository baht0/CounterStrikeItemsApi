namespace WebAdminPanel.Models.DTOs.Reference.ItemType
{
    public class ItemTypeUpdateDto : ReferenceUpdateDto, IReferenceUpdateDto
    {
        public List<Guid> Subtypes { get; set; } = [];
    }
}
