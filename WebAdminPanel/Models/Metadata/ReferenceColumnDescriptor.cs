using Microsoft.AspNetCore.Components;

namespace WebAdminPanel.Models.Metadata
{
    public class ReferenceColumnDescriptor<T>
    {
        public string Title { get; set; } = string.Empty;
        public Func<T, object?> ValueSelector { get; set; } = default!;
        public RenderFragment<T>? Template { get; set; } // Для кастомного рендера (например, цвет)
        public string? Width { get; set; }
        public Func<T, object?>? SortByFunc { get; set; } = default!;
    }
}
