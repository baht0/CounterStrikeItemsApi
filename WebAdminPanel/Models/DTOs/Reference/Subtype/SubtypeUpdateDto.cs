namespace WebAdminPanel.Models.DTOs.Reference.Subtype
{
    public class SubtypeUpdateDto : ReferenceUpdateDto, IReferenceUpdateDto
    {
        public List<Guid> ItemTypes { get; set; } = [];
    }
}
