using WebAdminPanel.Models.DTOs.Reference;

namespace WebAdminPanel.Models.DTOs.TypeSubtype
{
    public class TypeSubtypeDto
    {
        public ReferenceDto ItemType { get; set; } = null!;
        public List<ReferenceDto> Subypes { get; set; } = [];
    }
}
