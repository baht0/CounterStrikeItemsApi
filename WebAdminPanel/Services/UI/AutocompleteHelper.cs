using MudBlazor;
using WebAdminPanel.Models.DTOs.Reference;

namespace WebAdminPanel.Services.UI
{
    public static class AutocompleteHelper
    {
        // Универсальный поиск по объектам TDto
        public static Func<string, CancellationToken, Task<IEnumerable<TDto>>> GetSearchFunc<TDto>(List<TDto> items)
            where TDto : ReferenceDto
        {
            return (value, token) =>
            {
                IEnumerable<TDto> query = string.IsNullOrWhiteSpace(value)
                    ? items
                    : items.Where(i =>
                          i.Name.Contains(value, StringComparison.OrdinalIgnoreCase) ||
                          i.Slug.Contains(value, StringComparison.OrdinalIgnoreCase));

                return Task.FromResult(query.Take(200));
            };
        }

        // Преобразование выбранного объекта в отображаемую строку
        public static Func<TDto?, string> GetToStringFunc<TDto>(List<TDto> items)
            where TDto : ReferenceDto
        {
            return dto =>
            {
                if (dto == null) return string.Empty;
                return dto.Name;
            };
        }

        // Универсальный обработчик добавления выбранного объекта в коллекцию
        public static Action<TDto?> OnSelected<TDto>(ICollection<TDto> selected)
            where TDto : ReferenceDto
        {
            return dto =>
            {
                if (dto == null) return;
                if (!selected.Contains(dto))
                    selected.Add(dto);
            };
        }
        public static Action<TDto?> OnSelected<TDto>(
            ICollection<TDto> selected,
            MudAutocomplete<TDto> autocomplete)
            where TDto : ReferenceDto
        {
            return async dto =>
            {
                if (dto == null)
                {
                    await autocomplete.ClearAsync();
                    return;
                }

                if (!selected.Contains(dto))
                    selected.Add(dto);

                // очистка после выбора
                await autocomplete.ClearAsync();
            };
        }

        // Удаление объекта через MudChip
        public static Action<MudChip<TDto>> OnChipClose<TDto>(ICollection<TDto> selected)
            where TDto : ReferenceDto
        {
            return chip =>
            {
                if (chip.Value != null)
                    selected.Remove(chip.Value);
            };
        }
    }
}
