using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebAdminPanel.Components.Dialogs;
using WebAdminPanel.Contracts.Api;
using WebAdminPanel.Contracts.Api.References;
using WebAdminPanel.Models;
using WebAdminPanel.Models.DTOs.ItemCommons;
using WebAdminPanel.Models.DTOs.Reference;
using WebAdminPanel.Models.DTOs.Reference.Collection;
using WebAdminPanel.Models.DTOs.Reference.ItemType;
using WebAdminPanel.Models.DTOs.Reference.Subtype;
using WebAdminPanel.Services.UI;
using WebAdminPanel.ViewModels;

namespace WebAdminPanel.Pages
{
    public partial class ItemCommon
    {
        [Parameter] public string Slug { get; set; } = null!;

        private string PageTitle = "ItemCommon";
        private bool isLoading = true;
        private bool isEditMode;
        private bool isNotFound;

        // выпадающие списки ItemCommon
        private List<ItemTypeDto> Types = [];
        private List<SubtypeDto> Subtypes = [];
        private List<CollectionDto> Collections = [];
        private List<ReferenceDto> Tournaments = [];
        private List<ReferenceDto> Teams = [];
        private List<ReferenceDto> ProfessionalPlayers = [];

        private ItemCommonViewModel itemViewModel = null!;

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            try
            {
                Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
                // загружаем элемент
                if (Slug != "add")
                {
                    isEditMode = true;
                    var dto = await RefApiFactory.GetClient<IItemCommonApi>().GetBySlug(Slug);
                    itemViewModel = _mapper.Map<ItemCommonViewModel>(dto);
                    PageTitle = itemViewModel.Name;
                }
                else
                {
                    itemViewModel = new();
                    PageTitle = "Add New ItemCommon";
                }

                // загружаем справочники
                Collections = await RefApiFactory.GetClient<ICollectionApi>().GetAll();
                ProfessionalPlayers = await RefApiFactory.GetClient<IProfessionalPlayerApi>().GetAll();
                Tournaments = await RefApiFactory.GetClient<ITournamentApi>().GetAll();
                Teams = await RefApiFactory.GetClient<ITeamApi>().GetAll();
                Types = await RefApiFactory.GetClient<IItemTypeApi>().GetAll();
                Subtypes = await RefApiFactory.GetClient<ISubtypeApi>().GetAll();
            }
            catch (ApiBadRequestException ex)
            {
                Snackbar.Add("Bad request: init", Severity.Error);
                Console.WriteLine(ex.Message);
            }
            catch (ApiNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                isNotFound = true;
            }
            catch (Exception ex)
            {
                Snackbar.Add("Initialization error", Severity.Error);
                Console.WriteLine($"Error init: {ex.Message}");
            }
            finally
            {
                isLoading = false;
            }
        }

        //ItemCommon
        private async Task SubmitSaveItemAsync()
        {
            isLoading = true;
            try
            {
                var client = RefApiFactory.GetClient<IItemCommonApi>();
                var slug = itemViewModel.Slug;
                if (isEditMode)
                {
                    itemViewModel.Items = [.. itemViewModel.Items.Where(x => x.Status != ItemStatus.Deleted)];
                    var dto = _mapper.Map<ItemCommonUpdateBody>(itemViewModel);
                    await client.Update(dto);
                }
                else
                {
                    var dto = _mapper.Map<ItemCommonCreateBody>(itemViewModel);
                    var response = await client.Create(dto);
                    slug = response.GetFirstValue();
                }
                NavigationManager.NavigateTo($"/item/{slug}", true);
                Snackbar.Add("Saved successfully.", Severity.Success);
            }
            catch (ApiBadRequestException ex)
            {
                Snackbar.Add("Bad request: save item", Severity.Error);
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Save error: {ex.Message}", Severity.Error);
                Console.WriteLine($"Save error: {ex.Message}");
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }
        private async Task DeleteItemCommonDialogAsync()
        {
            var result = await DialogService.ShowDeleteDialogAsync("Are you sure you want to delete this ItemCommon?");
            if (result)
            {
                if (itemViewModel is null) return;
                var client = RefApiFactory.GetClient<IItemCommonApi>();
                try
                {
                    await client.Delete(itemViewModel!.Id);
                    NavigationManager.NavigateTo("/items", true);
                    Snackbar.Add($"ItemCommon deleted.");
                }
                catch (Exception ex)
                {
                    Snackbar.Add($"Error: {ex.Message}", Severity.Error);
                    Console.WriteLine($"Error init: {ex.Message}");
                }
            }
        }

        //Items
        private async Task ImageDialogAsync(ItemViewModel vm)
        {
            await DialogService.ShowImageDialogAsync(
                vm.Name,
                $"https://community.akamai.steamstatic.com/economy/image/{vm.ImageId}/360x360");
        }

        private async Task AddItemDialogAsync()
        {
            await DialogService.ShowFormDialogAsync<ItemDialog, ItemViewModel>(
                "Add New Item",
                new() { { x => x.Item, new ItemViewModel { Name = itemViewModel.Name, Status = ItemStatus.NewItem } } },
                onSuccess: newItem =>
                {
                    if (newItem != null)
                    {
                        itemViewModel.Items.Add(newItem);
                        Snackbar.Add($"New Item added.", Severity.Success);
                    }
                }
            );
        }
        private async Task GenerateItemDialogAsync()
        {
            await DialogService.ShowFormDialogAsync<GenerateItemsDialog, List<ItemViewModel>>(
                "Generate new items with parameters",
                new() { { x => x.ItemName, itemViewModel?.Name } },
                onSuccess: list =>
                {
                    if (list != null)
                    {
                        itemViewModel!.Items.AddRange(list);
                        Snackbar.Add($"New Items generated and added.", Severity.Success);
                    }
                }
            );
        }
        private async Task EditDialogAsync(ItemViewModel vm)
        {
            await DialogService.ShowFormDialogAsync<ItemDialog, ItemViewModel>(
                "Edit Item",
                new() { 
                    { x => x.Item, vm },
                    { x => x.ItemCommonName, itemViewModel.Name }
                },
                onSuccess: item =>
                {
                    if (item != null)
                    {
                        var index = itemViewModel.Items.FindIndex(x => x.Id == vm.Id);
                        if (index >= 0)
                        {
                            itemViewModel.Items[index] = item;
                            Snackbar.Add($"Changes applied.", Severity.Info);
                        }
                    }
                }
            );
        }
        private async Task DeleteItemDialogAsync(ItemViewModel vm)
        {
            var result = await DialogService.ShowDeleteDialogAsync("Are you sure you want to delete this item?");
            if (result)
            {
                if (vm.Status == ItemStatus.NewItem)
                    itemViewModel?.Items.Remove(vm);
                else
                {
                    vm.Status = ItemStatus.Deleted;
                    Snackbar.Add($"The deletion will occur upon saving.", Severity.Warning);
                }
            }
        }

        //Containers
        private async Task AddContainerDialogAsync()
        {
            await DialogService.ShowFormDialogAsync<ContainerDialog, ReferenceViewModel>(
                title: "New Container",
                parameters: new() 
                {
                    { x => x.Container, new() { Status = ItemStatus.NewItem } },
                },
                onSuccess: vm =>
                {
                    if (vm != null)
                    {
                        itemViewModel!.Containers.Add(vm);
                        Snackbar.Add($"New Container added.", Severity.Success);
                    }
                }
            );
        }
        private async Task EditContainerDialogAsync(ReferenceViewModel vm)
        {
            await DialogService.ShowFormDialogAsync<ContainerDialog, ReferenceViewModel>(
                title: "Edit Found In Container",
                parameters: new() { { x => x.Container, vm } },
                onSuccess: container =>
                {
                    if (container != null)
                    {
                        var index = itemViewModel.Containers.FindIndex(x => x.Id == vm.Id);
                        if (index >= 0)
                        {
                            itemViewModel.Containers[index] = container;
                            Snackbar.Add($"Changes applied.", Severity.Info);
                        }
                    }
                }
            );
        }
        private async Task DeleteContainerDialogAsync(ReferenceViewModel vm)
        {
            var result = await DialogService.ShowDeleteDialogAsync("Are you sure you want to delete this container entry?");
            if (result)
            {
                if (vm.Status == ItemStatus.NewItem)
                    itemViewModel?.Containers.Remove(vm);
                else
                {
                    vm.Status = ItemStatus.Deleted;
                    Snackbar.Add($"The deletion will occur upon saving.", Severity.Warning);
                }
            }
        }

        //General
        private static string GetTooltipText(IEditElement item)
        {
            return item.Status switch
            {
                ItemStatus.NewItem => "This item will be added.",
                ItemStatus.Edited => "The item has been modified.",
                ItemStatus.Deleted => "The item will be deleted.",
                _ => string.Empty,
            };
        }
        private static RenderFragment GetStatusIcon(IEditElement item)
        {
            return __builder =>
            {
                var icon = item.Status switch
                {
                    ItemStatus.NewItem => Icons.Material.Outlined.AddTask,
                    ItemStatus.Edited => Icons.Material.Outlined.Warning,
                    ItemStatus.Deleted => Icons.Material.Outlined.Error,
                    _ => string.Empty,
                };

                var color = item.Status switch
                {
                    ItemStatus.NewItem => Color.Success,
                    ItemStatus.Edited => Color.Warning,
                    ItemStatus.Deleted => Color.Error,
                    _ => Color.Default,
                };

                var visibilityClass = item.Status != null
                    ? "visible"
                    : "invisible";

                __builder.OpenComponent<MudIcon>(0);
                __builder.AddAttribute(1, "Size", Size.Medium);
                __builder.AddAttribute(2, "Icon", icon);
                __builder.AddAttribute(3, "Color", color);
                __builder.AddAttribute(4, "Class", $"pa-0 {visibilityClass}");
                __builder.CloseComponent();
            };
        }
    }
}
