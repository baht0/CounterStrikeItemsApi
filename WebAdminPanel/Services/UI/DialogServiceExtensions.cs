using Microsoft.AspNetCore.Components;
using MudBlazor;
using WebAdminPanel.Components.Dialogs;
using WebAdminPanel.ViewModels;

namespace WebAdminPanel.Services.UI
{
    public static class DialogServiceExtensions
    {
        //form
        public static async Task<TModel?> ShowFormDialogAsync<TDialog, TModel>(
            this IDialogService dialogService,
            string title,
            DialogParameters<TDialog> parameters,
            Action<TModel?>? onSuccess = null) where TDialog : ComponentBase
        {
            var options = new DialogOptions
            {
                CloseButton = true,
                BackdropClick = false,
                CloseOnEscapeKey = false,
                MaxWidth = MaxWidth.Small,
                FullWidth = true
            };
            
            var dialog = await dialogService.ShowAsync<TDialog>(title, parameters, options ?? new DialogOptions());
            var result = await dialog.Result;
            if (result?.Canceled == false && result.Data is TModel data)
            {
                onSuccess?.Invoke(data);
                return data;
            }
            return default;
        }

        //Delete
        public static async Task<bool> ShowDeleteDialogAsync(
            this IDialogService dialogService,
            string text)
        {
            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                CloseButton = true,
                MaxWidth = MaxWidth.ExtraSmall,
                FullWidth = true
            };
            var parameters = new DialogParameters<DeleteDialog>
            {
                { x => x.ContentText, text },
            };
            var dialog = await dialogService.ShowAsync<DeleteDialog>("Delete", parameters, options);
            var result = await dialog.Result;

            return result?.Canceled == false && result.Data is bool isConfirmed && isConfirmed;
        }

        //image
        public static async Task ShowImageDialogAsync(
            this IDialogService dialogService,
            string title,
            string? imageUrl)
        {
            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                MaxWidth = MaxWidth.ExtraSmall,
                FullWidth = true
            };
            var parameters = new DialogParameters<ImageDialog>
            {
                { x => x.ImageUrl, imageUrl },
            };
            var dialog = await dialogService.ShowAsync<ImageDialog>(title, parameters, options);
        }
    }
}
