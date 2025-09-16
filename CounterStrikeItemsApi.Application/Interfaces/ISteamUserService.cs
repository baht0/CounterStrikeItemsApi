using CounterStrikeItemsApi.Application.DTOs;
using CounterStrikeItemsApi.Application.DTOs.SteamUsers;
using CounterStrikeItemsApi.Domain.Models;

namespace CounterStrikeItemsApi.Application.Interfaces
{
    public interface ISteamUserService
    {
        Task<PagedResult<SteamUser>> GetUsers(SteamUserFilterQuery query);
        Task<SteamUser> GetOrCreateOrUpdateAsync(string steamId, string nickname, string avatarUrl);

        Task UpdateExistUserAsync(SteamUserUpdate user);
        Task<bool> DeleteAsync(Guid id);

        Task<(string Nickname, string AvatarUrl)> GetSteamProfileAsync(string steamId);
    }
}
