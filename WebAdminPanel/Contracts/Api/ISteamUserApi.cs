using CounterStrikeItemsApi.Application.DTOs.SteamUsers;
using Refit;
using WebAdminPanel.Models.DTOs;

namespace WebAdminPanel.Contracts.Api
{
    public interface ISteamUserApi
    {
        [Get("/steamuser/users")]
        [QueryUriFormat(UriFormat.Unescaped)]
        Task<PagedResult<SteamUserDto>> GetSearch(
            [Query] SteamUserFilterQuery? query = null);

        [Put("/steamuser/update")]
        Task Update([Body] SteamUserUpdate body);

        [Delete("/steamuser/delete/{id}")]
        Task Delete(Guid id);
    }
}