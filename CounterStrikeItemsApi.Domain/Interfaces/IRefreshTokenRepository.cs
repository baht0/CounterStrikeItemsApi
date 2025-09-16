using CounterStrikeItemsApi.Domain.Models;

namespace CounterStrikeItemsApi.Domain.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task AddAsync(RefreshToken refreshToken);
        void Update(RefreshToken refreshToken);
        Task RevokeTokensForUserAsync(Guid userId);
        Task SaveChangesAsync();
    }
}
