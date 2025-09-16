using CounterStrikeItemsApi.Domain.Models;

namespace CounterStrikeItemsApi.Application.Interfaces
{
    public interface ITokenService
    {
        Task<(string accessToken, string refreshToken)> GenerateTokensAsync(SteamUser user);
        Task<(string accessToken, string refreshToken)> RefreshAsync(string refreshToken);
        Task RevokeRefreshTokenAsync(string refreshToken);
    }
}
