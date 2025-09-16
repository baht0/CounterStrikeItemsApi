using System.ComponentModel.DataAnnotations;

namespace CounterStrikeItemsApi.Application.DTOs.Reference
{
    public class ReferenceCreateDto
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = null!;
    }
}
