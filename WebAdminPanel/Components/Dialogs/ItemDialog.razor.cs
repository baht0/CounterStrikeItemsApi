using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebAdminPanel.Contracts.Api.References;
using WebAdminPanel.Models.DTOs.Reference;
using WebAdminPanel.ViewModels;

namespace WebAdminPanel.Components.Dialogs
{
    public partial class ItemDialog
    {
        [CascadingParameter] IMudDialogInstance MudDialog { get; set; } = default!;
        [Parameter] public ItemViewModel Item { get; set; } = default!;
        [Parameter] public string ItemCommonName { get; set; } = default!;

        // выпадающие списки Items
        private List<ReferenceDto> Exteriors = [];
        private List<ReferenceColorDto> Categories = [];
        private List<ReferenceColorDto> Qualities = [];
        private List<ReferenceColorDto> GraffitiColors = [];

        protected override async Task OnInitializedAsync()
        {
            // загружаем справочники
            Exteriors = await RefApiFactory.GetClient<IExteriorApi>().GetAll();
            Categories = await RefApiFactory.GetClient<ICategoryApi>().GetAll();
            Qualities = await RefApiFactory.GetClient<IQualityApi>().GetAll();
            GraffitiColors = await RefApiFactory.GetClient<IGraffitiColorApi>().GetAll();

            StateHasChanged();
        }
        private void Submit()
        {
            string itemName = ItemCommonName;
            Item.ImageId = Item.ImageId?
                .Replace("https://community.fastly.steamstatic.com/economy/image/", string.Empty)
                .Replace("/360fx360f", string.Empty);

            var exteriorName = Exteriors.Find(x => x.Id == Item.ExteriorId)?.Name;
            Item.ExteriorName = exteriorName;

            var categoryDto = Categories.Find(x => x.Id == Item.CategoryId);
            if (categoryDto != null)
            {
                Item.CategoryName = categoryDto.Name;
                Item.CategoryHexColor = categoryDto.HexColor;
            }
            var qualityDto = Qualities.Find(x => x.Id == Item.QualityId);
            if (qualityDto != null)
            {
                Item.QualityName = qualityDto.Name;
                Item.QualityHexColor = qualityDto.HexColor;
            }
            var colorDto = GraffitiColors.Find(x => x.Id == Item.GraffitiColorId);
            if (colorDto != null)
            {
                Item.GraffitiColorName = colorDto.Name;
                Item.GraffitiColorHexColor = colorDto.HexColor;
            }

            // Создаем/пересоздаем имя
            var parts = new List<string>();

            if (!string.IsNullOrWhiteSpace(categoryDto?.Name)
                && !categoryDto.Name.Equals("normal", StringComparison.CurrentCultureIgnoreCase))
                parts.Add(categoryDto.Name);

            parts.Add(itemName);

            if (!string.IsNullOrWhiteSpace(exteriorName)
                && !exteriorName.Equals("not painted", StringComparison.CurrentCultureIgnoreCase))
                parts[^1] += $" ({exteriorName})";

            if (!string.IsNullOrWhiteSpace(colorDto?.Name) 
                && colorDto.Name.Contains("multicolor", StringComparison.CurrentCultureIgnoreCase))
                parts[^1] += $" ({colorDto.Name})";

            Item.Name = string.Join(" ", parts);

            Item.Status = Item.Status != ItemStatus.NewItem ? ItemStatus.Edited : Item.Status;
            MudDialog.Close(DialogResult.Ok(Item));
        }
    }
}
