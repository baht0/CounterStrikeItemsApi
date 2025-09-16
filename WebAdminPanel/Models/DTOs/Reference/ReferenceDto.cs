using System.ComponentModel.DataAnnotations;

namespace WebAdminPanel.Models.DTOs.Reference
{
    public class ReferenceDto
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
    }
}
