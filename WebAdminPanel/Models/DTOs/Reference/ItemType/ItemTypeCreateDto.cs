namespace WebAdminPanel.Models.DTOs.Reference.ItemType
{
    public class ItemTypeCreateDto : ReferenceCreateDto
    {
        public List<Guid> Subtypes { get; set; } = [];
    }
}
