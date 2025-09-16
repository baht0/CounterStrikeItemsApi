using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebAdminPanel.Contracts.Api;
using WebAdminPanel.ViewModels;

namespace WebAdminPanel.Components.Dialogs
{
    public partial class ContainerDialog
    {
        [CascadingParameter] IMudDialogInstance MudDialog { get; set; } = default!;
        [Parameter] public ReferenceViewModel Container { get; set; } = default!;

        private ReferenceViewModel? selectedContainer;

        private List<ReferenceViewModel> Containers = [];
        private ItemStatus? _itemStatus;

        protected override async Task OnInitializedAsync()
        {
            // загружаем Containers из ItemCommons
            var dtoList = await RefApiFactory.GetClient<IItemCommonApi>().GetContainers();
            Containers = _mapper.Map<List<ReferenceViewModel>>(dtoList);

            _itemStatus = Container.Status;
            selectedContainer = Container;

            StateHasChanged();
        }

        private void Submit()
        {
            if (selectedContainer != null)
            {
                Container.Id = selectedContainer.Id;
                Container.Name = selectedContainer.Name;
                Container.Slug = selectedContainer.Slug;
            }
            Container.Status = _itemStatus != ItemStatus.NewItem ? ItemStatus.Edited : _itemStatus;
            MudDialog.Close(DialogResult.Ok(Container));
        }
    }
}
