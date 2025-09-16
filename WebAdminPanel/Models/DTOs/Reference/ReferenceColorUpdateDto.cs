namespace WebAdminPanel.Models.DTOs.Reference
{
    public class ReferenceColorUpdateDto : ReferenceUpdateDto, IReferenceUpdateDto
    {
        public string? HexColor { get; set; }
    }
}
