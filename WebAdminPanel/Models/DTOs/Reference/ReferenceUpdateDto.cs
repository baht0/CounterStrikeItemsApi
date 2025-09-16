namespace WebAdminPanel.Models.DTOs.Reference
{
    public class ReferenceUpdateDto : IReferenceUpdateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
