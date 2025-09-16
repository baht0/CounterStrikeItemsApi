using Microsoft.AspNetCore.Components;

namespace WebAdminPanel.Models.Metadata
{
    public class ReferenceEntityDescriptor<TDto, TCreateDto, TUpdateDto>
    {
        public string Title { get; set; } = string.Empty;

        public Func<Task<List<TDto>>> GetAllAsync { get; set; } = default!;
        public Func<TCreateDto, Task> CreateAsync { get; set; } = default!;
        public Func<TUpdateDto, Task> UpdateAsync { get; set; } = default!;
        public Func<Guid, Task> DeleteAsync { get; set; } = default!;

        public Func<TDto, TUpdateDto> MapToUpdateDto { get; set; } = default!;
        public Func<TUpdateDto, TCreateDto> MapToCreateDto { get; set; } = default!;

        public RenderFragment<TUpdateDto> EditDialog { get; set; } = default!;

        public List<ReferenceColumnDescriptor<TDto>> Columns { get; set; } = [];
    }
}
