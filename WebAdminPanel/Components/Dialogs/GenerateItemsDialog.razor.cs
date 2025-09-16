using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebAdminPanel.Contracts.Api.References;
using WebAdminPanel.Models.DTOs.Reference;
using WebAdminPanel.ViewModels;

namespace WebAdminPanel.Components.Dialogs
{
    public partial class GenerateItemsDialog
    {
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
        [Parameter] public string ItemName { get; set; } = default!;

        private bool isLoading = true;

        // выпадающие списки Items
        private List<ReferenceDto> Exteriors = [];
        private List<ReferenceColorDto> Categories = [];
        private List<ReferenceColorDto> Qualities = [];
        private List<ReferenceColorDto> GraffitiColors = [];

        private IEnumerable<Guid?> _selectedExteriors = new HashSet<Guid?>();
        private IEnumerable<Guid?> _selectedCategories = new HashSet<Guid?>();
        private IEnumerable<Guid?> _selectedQualities = new HashSet<Guid?>();
        private IEnumerable<Guid?> _selectedGraffitiColors = new HashSet<Guid?>();

        private List<ItemViewModel> _itemViewModels = [];

        protected override async Task OnInitializedAsync()
        {
            Exteriors = await RefApiFactory.GetClient<IExteriorApi>().GetAll();
            Categories = await RefApiFactory.GetClient<ICategoryApi>().GetAll();
            Qualities = await RefApiFactory.GetClient<IQualityApi>().GetAll();
            GraffitiColors = await RefApiFactory.GetClient<IGraffitiColorApi>().GetAll();

            isLoading = false;
        }
        private static string GetMultiSelectionText(List<string> selectedValues)
            => selectedValues.Count > 0 ? $"Selected: {selectedValues.Count}" : string.Empty;

        private async Task Submit()
        {
            await Task.Run(AddCombinations);

            MudDialog.Close(DialogResult.Ok(_itemViewModels));
        }
        private void AddCombinations()
        {
            isLoading = true;
            try
            {
                if (!_selectedExteriors.Any() && !_selectedCategories.Any() && !_selectedQualities.Any() && !_selectedGraffitiColors.Any())
                    return;

                var exteriors = _selectedExteriors.Any() ? _selectedExteriors : new HashSet<Guid?>() { null };
                var categories = _selectedCategories.Any() ? _selectedCategories : new HashSet<Guid?>() { null };
                var qualities = _selectedQualities.Any() ? _selectedQualities : new HashSet<Guid?>() { null };
                var graffitiColors = _selectedGraffitiColors.Any() ? _selectedGraffitiColors : new HashSet<Guid?>() { null };

                foreach (var category in categories)
                {
                    foreach (var quality in qualities)
                    {
                        foreach (var exterior in exteriors)
                        {
                            foreach (var graffiti in graffitiColors)
                            {
                                var item = new ItemViewModel
                                {
                                    ExteriorId = exterior,
                                    CategoryId = category,
                                    QualityId = quality,
                                    GraffitiColorId = graffiti,
                                    Status = ItemStatus.NewItem
                                };

                                // Временные переменные для частей имени
                                string nameBase = ItemName;
                                string? exteriorName = null;
                                string? categoryName = null;
                                string? graffitiColorName = null;

                                // Exterior
                                if (exterior != null)
                                {
                                    exteriorName = Exteriors.Find(x => x.Id == exterior)!.Name;
                                    item.ExteriorName = exteriorName;
                                }

                                // Category
                                if (category != null)
                                {
                                    var cat = Categories.Find(x => x.Id == category)!;
                                    categoryName = cat.Name;
                                    item.CategoryName = cat.Name;
                                    item.CategoryHexColor = cat.HexColor;
                                }

                                // Quality
                                if (quality != null)
                                {
                                    var qual = Qualities.Find(x => x.Id == quality)!;
                                    item.QualityName = qual.Name;
                                    item.QualityHexColor = qual.HexColor;
                                }

                                // Graffiti
                                if (graffiti != null)
                                {
                                    var graff = GraffitiColors.Find(x => x.Id == graffiti)!;
                                    graffitiColorName = graff.Name;
                                    item.GraffitiColorName = graff.Name;
                                    item.GraffitiColorHexColor = graff.HexColor;
                                }

                                // Формируем имя
                                var parts = new List<string>();

                                if (!string.IsNullOrWhiteSpace(categoryName) &&
                                    !categoryName.Equals("normal", StringComparison.CurrentCultureIgnoreCase))
                                    parts.Add(categoryName);

                                parts.Add(nameBase);

                                if (!string.IsNullOrWhiteSpace(exteriorName) &&
                                    !exteriorName.Equals("not painted", StringComparison.CurrentCultureIgnoreCase))
                                    parts[^1] += $" ({exteriorName})"; // добавляем к последней части

                                if (!string.IsNullOrWhiteSpace(graffitiColorName) &&
                                    graffitiColorName.Contains("multicolor", StringComparison.CurrentCultureIgnoreCase))
                                    parts[^1] += $" ({graffitiColorName})";

                                item.Name = string.Join(" ", parts);

                                _itemViewModels.Add(item);
                            }
                        }
                    }
                }
            }
            finally
            {
                isLoading = false;
            }
        }
    }
}
