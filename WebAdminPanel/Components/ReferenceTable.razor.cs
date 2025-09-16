using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Linq.Expressions;
using System.Reflection;
using WebAdminPanel.Components.Dialogs;
using WebAdminPanel.Models;
using WebAdminPanel.Models.Metadata;
using WebAdminPanel.Services.UI;

namespace WebAdminPanel.Components
{
    public partial class ReferenceTable<TDto, TCreateDto, TUpdateDto>
    {
        [Parameter] public ReferenceEntityDescriptor<TDto, TCreateDto, TUpdateDto> Descriptor { get; set; } = default!;

        private List<TDto> _items = [];
        private IEnumerable<TDto> FilteredItems => string.IsNullOrWhiteSpace(_search) ? _items : _items.Where(MatchesSearch);

        private string _search = string.Empty;

        private List<ReferenceColumnDescriptor<TDto>> _columns = [];

        protected override void OnInitialized()
        {
            Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
        }
        protected override async Task OnParametersSetAsync()
        {
            _columns = BuildColumns();
            await ReloadAsync();
        }

        // Генерация компилируемого геттера для свойства (быстрее, чем reflection GetValue)
        private static Func<TDto, object?> CreatePropertyGetter(PropertyInfo prop)
        {
            var param = Expression.Parameter(typeof(TDto), "x");
            Expression access = Expression.Property(Expression.Convert(param, prop.DeclaringType!), prop);
            var convert = Expression.Convert(access, typeof(object));
            return Expression.Lambda<Func<TDto, object?>>(convert, param).Compile();
        }

        // Генерация дефолтных колонок (Id, Name, Slug) — только если такие свойства есть
        private static List<ReferenceColumnDescriptor<TDto>> BuildDefaultColumns()
        {
            var type = typeof(TDto);
            var defaultNames = new[] { "Id", "Name", "Slug" };
            var cols = new List<ReferenceColumnDescriptor<TDto>>();

            foreach (var name in defaultNames)
            {
                var prop = type.GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
                if (prop == null) continue;

                cols.Add(new ReferenceColumnDescriptor<TDto>
                {
                    Title = prop.Name,
                    ValueSelector = CreatePropertyGetter(prop),
                    Width = prop.Name == "Id" ? "400px" : null,
                    SortByFunc = CreatePropertyGetter(prop),
                });
            }

            return cols;
        }

        // Собираем итоговые колонки: дефолтные + extra (Descriptor.Columns)
        private List<ReferenceColumnDescriptor<TDto>> BuildColumns()
        {
            var defaults = ReferenceTable<TDto, TCreateDto, TUpdateDto>.BuildDefaultColumns();

            // Дескриптор может уже содержать дополнительные колонки в Descriptor.Columns
            var extras = Descriptor.Columns ?? [];

            // Если нужно — избегаем дубликатов по Title
            var final = new List<ReferenceColumnDescriptor<TDto>>(defaults);

            foreach (var ex in extras)
            {
                // если колонка с таким Title уже есть — заменим или пропустим; здесь — добавляем, если нет
                if (!final.Any(c => string.Equals(c.Title, ex.Title, StringComparison.OrdinalIgnoreCase)))
                    final.Add(ex);
                else // либо заменить существующую:
                {
                    var idx = final.FindIndex(c => string.Equals(c.Title, ex.Title, StringComparison.OrdinalIgnoreCase));
                    final[idx] = ex;
                }
            }
            return final;
        }

        private async Task ReloadAsync()
        {
            try
            {
                _items = await Descriptor.GetAllAsync();
            }
            catch (ApiBadRequestException ex)
            {
                Snackbar.Add("Bad request: init", Severity.Error);
                Console.WriteLine(ex.Message);
            }
            catch (ApiNotFoundException ex)
            {
                Snackbar.Add($"Not Found: init", Severity.Error);
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Initialization error: {ex.Message}", Severity.Error);
                Console.WriteLine($"Error init: {ex.Message}");
            }
            StateHasChanged();
        }

        private bool MatchesSearch(TDto item)
        {
            if (string.IsNullOrWhiteSpace(_search)) return true;

            foreach (var col in _columns)
            {
                var val = col.ValueSelector?.Invoke(item);
                if (val is null) continue;

                // Строки
                if (val is string s && s.Contains(_search, StringComparison.OrdinalIgnoreCase))
                    return true;

                // Guid и прочее
                var str = val.ToString();
                if (!string.IsNullOrEmpty(str) && str.Contains(_search, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
        private Task OnSearchChanged(string value)
        {
            _search = value;
            StateHasChanged();
            return Task.CompletedTask;
        }

        private async Task Create()
        {
            var newModel = Descriptor.MapToUpdateDto(Activator.CreateInstance<TDto>());
            await DialogService.ShowFormDialogAsync<ReferenceDialog<TUpdateDto>, TUpdateDto>(
                "Add New Value",
                parameters: new DialogParameters<ReferenceDialog<TUpdateDto>>()
                {
                    { x => x.Model, newModel},
                    { x => x.ChildContent, Descriptor.EditDialog }
                },
                onSuccess: async vm =>
                {
                    await Descriptor.CreateAsync(Descriptor.MapToCreateDto(vm!));
                    await ReloadAsync();
                    Snackbar.Add($"The new value has been added and saved.", Severity.Success);
                }
            );
        }
        private async Task Edit(TDto dto)
        {
            var model = Descriptor.MapToUpdateDto(dto);
            await DialogService.ShowFormDialogAsync<ReferenceDialog<TUpdateDto>, TUpdateDto>(
                "Edit Value",
                parameters: new DialogParameters<ReferenceDialog<TUpdateDto>>()
                {
                    { x => x.Model, model},
                    { x => x.ChildContent, Descriptor.EditDialog }
                },
                onSuccess: async vm =>
                {
                    await Descriptor.UpdateAsync(vm!);
                    await ReloadAsync();
                    Snackbar.Add($"Changes applied.", Severity.Info);
                }
            );
        }
        private async Task Delete(TDto dto)
        {
            var result = await DialogService.ShowDeleteDialogAsync(
                "Are you sure you want to delete this value? This will happen immediately and cannot be restored.");
            if (result)
            {
                var idProp = typeof(TDto).GetProperty("Id")?.GetValue(dto);
                if (idProp is Guid id)
                {
                    await Descriptor.DeleteAsync(id);
                    await ReloadAsync();
                    Snackbar.Add($"The row has been removed.");
                }
            }
        }
    }
}
