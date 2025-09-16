using CounterStrikeItemsApi.Application.DTOs.SteamUsers;
using Microsoft.AspNetCore.WebUtilities;
using MudBlazor;
using WebAdminPanel.Contracts.Api;
using WebAdminPanel.Models;
using WebAdminPanel.Models.DTOs;
using WebAdminPanel.Services.UI;

namespace WebAdminPanel.Pages
{
    public partial class Users
    {
        private bool _isInitialized = false;
        private bool _isFirstLoad = true;
        protected bool isLoading = true;

        private MudTable<SteamUserDto> table = null!;
        private SteamUserFilterQuery Query = new();
        protected PagedResult<SteamUserDto> SteamUsers = new();
        private string SearchText { get; set; } = string.Empty;

        protected override void OnInitialized()
        {
            Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
        }
        protected override async Task OnParametersSetAsync()
        {
            var uri = Navigation.ToAbsoluteUri(Navigation.Uri);
            var queryParams = QueryHelpers.ParseQuery(uri.Query);

            if (queryParams.TryGetValue("page", out var pageValue) && int.TryParse(pageValue, out var page))
            {
                Query.Page = page;
            }
            if (queryParams.TryGetValue("pageSize", out var pageSizeValue) && int.TryParse(pageSizeValue, out var pageSize))
            {
                Query.PageSize = pageSize;
                table?.SetRowsPerPage(Query.PageSize);
            }
            if (queryParams.TryGetValue("nickname", out var searchValue))
                Query.Nickname = searchValue!;

            _isInitialized = true; // теперь можно загружать данные

            if (table is not null)
                await table.ReloadServerData();
        }
        private static string BuildQuery(SteamUserFilterQuery query)
        {
            var queryParams = new List<string>
            {
                $"page={query.Page}",
                $"pageSize={query.PageSize}"
            };

            if (!string.IsNullOrWhiteSpace(query.Nickname))
                queryParams.Add($"nickname={Uri.EscapeDataString(query.Nickname)}");

            return "users?" + string.Join("&", queryParams);
        }

        private async Task<TableData<SteamUserDto>> LoadServerData(TableState state, CancellationToken token)
        {
            isLoading = true;

            try
            {
                if (!_isInitialized)
                    return new TableData<SteamUserDto>();

                if (!_isFirstLoad)
                {
                    Query.Page = state.Page + 1;
                    Query.PageSize = state.PageSize;
                }
                else
                {
                    state.Page = Query.Page - 1;
                    table.SetRowsPerPage(Query.PageSize);
                    _isFirstLoad = false;
                }

                // Обновляем URL без перезагрузки страницы
                var newUri = BuildQuery(Query);
                Navigation?.NavigateTo(newUri, forceLoad: false);
                var result = await RefApiFactory.GetClient<ISteamUserApi>().GetSearch(Query);

                //sortiring
                var users = state.SortLabel switch
                {
                    "name_fld" => [.. result.Rows.OrderByDirection(state.SortDirection, x => x.Nickname)],
                    "ban_fld" => [.. result.Rows.OrderByDirection(state.SortDirection, x => x.IsBanned)],
                    "upd_fld" => [.. result.Rows.OrderByDirection(state.SortDirection, x => x.UpdatedAt)],
                    "crt_fld" => [.. result.Rows.OrderByDirection(state.SortDirection, x => x.CreatedAt)],
                    _ => result.Rows,
                };

                return new TableData<SteamUserDto>
                {
                    Items = users,
                    TotalItems = result.TotalRows
                };
            }
            catch (ApiBadRequestException ex)
            {
                Snackbar.Add($"Bad request: {ex.Message}", Severity.Error);
                Console.WriteLine(ex.Message);
                return new();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Unexpected error: {ex.Message}", Severity.Error);
                Console.WriteLine(ex);
                return new();
            }
            finally
            {
                isLoading = false;
            }
        }
        private async Task ReloadTable()
        {
            Query = new()
            {
                Nickname = SearchText,
                Page = 1
            };
            await table.ReloadServerData();
            Snackbar.Add("Table reloaded.", Severity.Success);
        }
        private async Task ClearFilters()
        {
            SearchText = string.Empty;
            Query = new();

            Snackbar.Add("Filters cleared.", Severity.Info);
            await table.ReloadServerData();
        }

        private async Task ImageDialogAsync(SteamUserDto dto)
        {
            await DialogService.ShowImageDialogAsync(
                dto.Nickname,
                dto.AvatarUrl);
        }

        private async Task BanUserDialogAsync(SteamUserDto dto)
        {
            var title = $"Ban user";
            var text = $"Block access for user '{dto.Nickname}'?";
            var snackbar = "The user has been blocked.";

            if (dto.IsBanned)
            {
                title = "Unban user";
                text = $"Unblock user '{dto.Nickname}'?";
                snackbar = "The user has been unblocked.";
            }

            var result = await DialogService.ShowMessageBox(
                title, text, 
                yesText: "Yes", noText: "No");
            if (result == true)
            {
                var updateDto = new SteamUserUpdate()
                {
                    Id = dto.Id,
                    IsBanned = !dto.IsBanned
                };
                await RefApiFactory.GetClient<ISteamUserApi>().Update(updateDto);
                await table.ReloadServerData();
                Snackbar.Add(snackbar, Severity.Info);
            }
        }
        private async Task DeleteUserDialogAsync(SteamUserDto dto)
        {
            var result = await DialogService.ShowDeleteDialogAsync("Are you sure you want to delete this user's data?");
            if (result)
            {
                await RefApiFactory.GetClient<ISteamUserApi>().Delete(dto.Id);
                await table.ReloadServerData();
                Snackbar.Add($"User data has been deleted.");
            }
        }
    }
}
